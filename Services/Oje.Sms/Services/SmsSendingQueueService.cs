using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Security.Interfaces;
using Oje.Sms.Interfaces;
using Oje.Sms.Models.DB;
using Oje.Sms.Models.View;
using Oje.Sms.Services.EContext;

namespace Oje.Sms.Services
{
    public class SmsSendingQueueService : ISmsSendingQueueService
    {
        readonly SmsDBContext db = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;
        readonly ISmsSenderService SmsSenderService = null;
        readonly ISmsSendingQueueErrorService SmsSendingQueueErrorService = null;
        readonly ISmsValidationHistoryService SmsValidationHistoryService = null;
        readonly IUserService UserService = null;
        readonly ISmsTemplateService SmsTemplateService = null;
        readonly IBlockLoginUserService BlockLoginUserService = null;
        public SmsSendingQueueService(
                SmsDBContext db,
                IHttpContextAccessor HttpContextAccessor,
                ISmsSenderService SmsSenderService,
                ISmsSendingQueueErrorService SmsSendingQueueErrorService,
                ISmsValidationHistoryService SmsValidationHistoryService,
                IUserService UserService,
                ISmsTemplateService SmsTemplateService,
                IBlockLoginUserService BlockLoginUserService
            )
        {
            this.db = db;
            this.HttpContextAccessor = HttpContextAccessor;
            this.SmsSenderService = SmsSenderService;
            this.SmsSendingQueueErrorService = SmsSendingQueueErrorService;
            this.SmsValidationHistoryService = SmsValidationHistoryService;
            this.UserService = UserService;
            this.SmsTemplateService = SmsTemplateService;
            this.BlockLoginUserService = BlockLoginUserService;
        }

        public void Create(SmsSendingQueue smsSendingQueue, int? siteSettingId, List<SmsLimit> smsLimits, bool? isWebsite)
        {
            var foundIp = HttpContextAccessor.GetIpAddress();
            if (foundIp == null)
                throw BException.GenerateNewException(BMessages.Ip_Format_Is_Not_Valid);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);

            smsSendingQueue.Ip1 = foundIp.Ip1;
            smsSendingQueue.Ip2 = foundIp.Ip2;
            smsSendingQueue.Ip3 = foundIp.Ip3;
            smsSendingQueue.Ip4 = foundIp.Ip4;
            smsSendingQueue.SiteSettingId = siteSettingId.Value;

            if (smsLimits != null)
            {
                var foundLimit = smsLimits.Where(t => t.type == smsSendingQueue.Type && t.isWebsite == isWebsite).FirstOrDefault();
                if (foundLimit != null)
                {
                    if (db.SmsSendingQueues.Count(t =>
                    t.CreateDate.Year == smsSendingQueue.CreateDate.Year && t.CreateDate.Month == smsSendingQueue.CreateDate.Month && t.CreateDate.Day == smsSendingQueue.CreateDate.Day &&
                    t.Ip1 == smsSendingQueue.Ip1 && t.Ip2 == smsSendingQueue.Ip2 && t.Ip3 == smsSendingQueue.Ip3 && t.Ip4 == smsSendingQueue.Ip4 && t.Type == smsSendingQueue.Type) >= foundLimit.value)
                        return;
                }

            }

            db.Entry(smsSendingQueue).State = EntityState.Added;
        }

        public object GetList(SmsSendingQueueMainGrid searchInput, int? siteSettingId)
        {
            if (searchInput == null)
                searchInput = new();

            var qureResult = db.SmsSendingQueues.getSiteSettingQuiry(HttpContextAccessor?.HttpContext?.GetLoginUser()?.canSeeOtherWebsites, siteSettingId);

            if (searchInput.type != null)
                qureResult = qureResult.Where(t => t.Type == searchInput.type);
            if (!string.IsNullOrEmpty(searchInput.mobile))
                qureResult = qureResult.Where(t => t.MobileNumber == searchInput.mobile);
            if (!string.IsNullOrEmpty(searchInput.createDate) && searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDate = searchInput.createDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.CreateDate.Year == targetDate.Year && t.CreateDate.Month == targetDate.Month && t.CreateDate.Day == targetDate.Day);
            }
            if (!string.IsNullOrEmpty(searchInput.lTryDate) && searchInput.lTryDate.ConvertPersianNumberToEnglishNumber().ToEnDate() != null)
            {
                var targetDate = searchInput.lTryDate.ConvertPersianNumberToEnglishNumber().ToEnDate().Value;
                qureResult = qureResult.Where(t => t.LastTryDate != null && t.LastTryDate.Value.Year == targetDate.Year && t.LastTryDate.Value.Month == targetDate.Month && t.LastTryDate.Value.Day == targetDate.Day);
            }
            if (searchInput.countTry != null)
                qureResult = qureResult.Where(t => t.CountTry == searchInput.countTry);
            if (searchInput.isSuccess != null)
                qureResult = qureResult.Where(t => t.IsSuccess == searchInput.isSuccess);
            if (!string.IsNullOrEmpty(searchInput.ip) && searchInput.ip.GetIpSections() != null)
            {
                var ipSections = searchInput.ip.GetIpSections();
                qureResult = qureResult.Where(t => t.Ip1 == ipSections.Ip1 && t.Ip2 == ipSections.Ip2 && t.Ip3 == ipSections.Ip3 && t.Ip4 == ipSections.Ip4);
            }
            if (!string.IsNullOrEmpty(searchInput.siteTitleMN2))
                qureResult = qureResult.Where(t => t.SiteSetting.Title.Contains(searchInput.siteTitleMN2));

            var row = searchInput.skip;

            return new
            {
                total = qureResult.Count(),
                data = qureResult
                    .OrderByDescending(t => t.Id)
                    .Skip(searchInput.skip)
                    .Take(searchInput.take)
                    .Select(t => new
                    {
                        id = t.Id,
                        type = t.Type,
                        mobile = t.MobileNumber,
                        createDate = t.CreateDate,
                        lTryDate = t.LastTryDate,
                        countTry = t.CountTry,
                        isSuccess = t.IsSuccess,
                        ip1 = t.Ip1,
                        ip2 = t.Ip2,
                        ip3 = t.Ip3,
                        ip4 = t.Ip4,
                        lastError = t.SmsSendingQueueErrors.OrderByDescending(tt => tt.CreateDate).Select(tt => tt.Description).FirstOrDefault(),
                        siteTitleMN2 = t.SiteSetting.Title
                    })
                    .ToList()
                    .Select(t => new
                    {
                        t.id,
                        row = ++row,
                        type = t.type.GetEnumDisplayName(),
                        t.mobile,
                        createDate = t.createDate.ToFaDate() + " " + t.createDate.ToString("hh:mm"),
                        lTryDate = t.lTryDate != null ? (t.lTryDate.ToFaDate() + " " + t.lTryDate.Value.ToString("hh:mm")) : "",
                        t.countTry,
                        isSuccess = t.isSuccess == true ? BMessages.Yes.GetEnumDisplayName() : BMessages.No.GetEnumDisplayName(),
                        ip = t.ip1 + "." + t.ip2 + "." + t.ip3 + "." + t.ip4,
                        t.lastError,
                        siteTitleMN2 = t.siteTitleMN2
                    })
                    .ToList()
            };
        }

        public object LoginWithSMS(RegLogSMSVM input, IpSections ipSections, int? siteSettingId)
        {
            LoginWithSMSValidation(input, ipSections, siteSettingId);

            var foundUser = UserService.GetBy(siteSettingId, input.username);

            int deffFromLastSendSecound = SmsValidationHistoryService.GetLastSecoundFor(SmsValidationHistoryType.RegisterLogin, ipSections, siteSettingId);
            if (deffFromLastSendSecound < 120)
                throw BException.GenerateNewException(string.Format(BMessages.Please_W8_X_Secound.GetEnumDisplayName(), (120 - deffFromLastSendSecound)), foundUser?.Id ?? 0);

            if (!BlockLoginUserService.IsValidDay(DateTime.Now, siteSettingId))
                throw BException.GenerateNewException(BMessages.UnknownError);

            if (foundUser != null && (foundUser.IsActive == false || foundUser.IsDelete == true))
                throw BException.GenerateNewException(BMessages.UnknownError, ApiResultErrorCode.ValidationError, foundUser?.Id ?? 0);

            var newCode = SmsValidationHistoryService.Create(ipSections, input.username, siteSettingId, SmsValidationHistoryType.RegisterLogin);

            List<SmsTemplate> foundTemplate = null;
            string smsMessage = "";

            UserNotificationType? curType = null;


            if (foundUser != null)
            {
                curType = UserNotificationType.Login;
                smsMessage = "کاربر گرامی " + foundUser.Firstname + " " + foundUser.Lastname + " رمز یک بار مصرف جهت ورود شما عبارت است از " + Environment.NewLine + newCode;
                foundTemplate = SmsTemplateService.GetBy(curType.Value, siteSettingId);
            }
            else
            {
                curType = UserNotificationType.Register;
                smsMessage = "کاربر گرامی لطفا جهت ثبت نام از این کد استفاده کنید " + Environment.NewLine + newCode;
                foundTemplate = SmsTemplateService.GetBy(curType.Value, siteSettingId);
            }

            if (foundTemplate != null && foundTemplate.Count > 0)
                smsMessage = GlobalServices.replaceKeyword(foundTemplate.Select(t => t.Description).FirstOrDefault(), null, newCode.ToString(), foundUser != null ? (foundUser.Firstname + " " + foundUser.Lastname) : null, null);

            Create(new SmsSendingQueue()
            {
                Body = smsMessage,
                CreateDate = DateTime.Now,
                MobileNumber = input.username,
                SiteSettingId = siteSettingId.ToIntReturnZiro(),
                Subject = curType.GetEnumDisplayName(),
                Type = curType.Value
            }, siteSettingId, GlobalConfig.GetSmsLimitFromConfig(), null);
            SaveChange();

            return ApiResult.GenerateNewResult(true, BMessages.Please_Enter_SMSCode, new
            {
                data = new { username = input.username },
                labels = new List<object>() { new { inputName = "code", labelText = "کد  به شماره  " + input.username + " ارسال گردید" } },
                stepId = "confirmSMS",
                countDownId = "tryAginButtonCD"
            });
        }

        private void LoginWithSMSValidation(RegLogSMSVM input, IpSections ipSections, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.username))
                throw BException.GenerateNewException(BMessages.Please_Enter_Mobile);
            if (!input.username.IsMobile())
                throw BException.GenerateNewException(BMessages.Invalid_Mobile_Number);
            if (ipSections == null)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
        }

        public void SaveChange()
        {
            db.SaveChanges();
        }

        public async Task SendSms()
        {
            var curDT = DateTime.Now.AddSeconds(-55);
            var allItems = db.SmsSendingQueues.Where(t => t.LastTryDate == null && t.IsSuccess == false && t.CountTry == 0).ToList();
            if (allItems.Count == 0)
                allItems = db.SmsSendingQueues.Where(t => t.LastTryDate != null && t.IsSuccess == false && curDT > t.LastTryDate && t.CountTry <= 2).ToList();
            foreach (var item in allItems)
                item.LastTryDate = DateTime.Now;

            db.SaveChanges();

            foreach (var item in allItems)
            {
                SmsResult resultSms = null;
                try
                {
                    resultSms = await SmsSenderService.Send(item.MobileNumber, item.Body, item.SiteSettingId);
                }
                catch (Exception ex)
                {
                    item.CountTry++;
                    item.IsSuccess = false;
                    SmsSendingQueueErrorService.Create(item.Id, DateTime.Now, ex.Message, null);
                    continue;
                };
                if (resultSms != null && resultSms.isSuccess == true)
                {
                    item.IsSuccess = true;
                    item.TraceCode = resultSms.traceCode;
                }
                else if (resultSms != null)
                {
                    item.CountTry++;
                    item.IsSuccess = false;
                    SmsSendingQueueErrorService.Create(item.Id, DateTime.Now, resultSms.message, resultSms.cId);
                }
                else
                {
                    item.CountTry++;
                    item.IsSuccess = false;
                    SmsSendingQueueErrorService.Create(item.Id, DateTime.Now, "علت خطا مشخص نمی باشد", null);
                }
            }

            db.SaveChanges();
        }

        public object ActiveCodeForResetPassword(RegLogSMSVM input, IpSections ipSections, int? siteSettingId)
        {
            ActiveCodeForResetPasswordValidation(input, ipSections, siteSettingId);

            var foundUser = UserService.GetBy(siteSettingId, input.username);

            int deffFromLastSendSecound = SmsValidationHistoryService.GetLastSecoundFor(SmsValidationHistoryType.ForgetPassword, ipSections, siteSettingId);
            if (deffFromLastSendSecound < 120)
                throw BException.GenerateNewException(string.Format(BMessages.Please_W8_X_Secound.GetEnumDisplayName(), (120 - deffFromLastSendSecound)), foundUser?.Id ?? 0);

            if (!BlockLoginUserService.IsValidDay(DateTime.Now, siteSettingId))
                throw BException.GenerateNewException(BMessages.UnknownError);

            if (foundUser != null && foundUser.IsActive == true && foundUser.IsDelete != true)
            {
                var newCode = SmsValidationHistoryService.Create(ipSections, input.username, siteSettingId, SmsValidationHistoryType.ForgetPassword);

                List<SmsTemplate> foundTemplate = null;
                string smsMessage = "";

                UserNotificationType curType = UserNotificationType.ForgetPassword;
                smsMessage = "کاربر گرامی " + foundUser.Firstname + " " + foundUser.Lastname + " رمز یک بار مصرف جهت ورود شما عبارت است از " + Environment.NewLine + newCode;
                foundTemplate = SmsTemplateService.GetBy(curType, siteSettingId);

                if (foundTemplate != null && foundTemplate.Count > 0)
                    smsMessage = GlobalServices.replaceKeyword(foundTemplate.Select(t => t.Description).FirstOrDefault(), null, newCode.ToString(), foundUser != null ? (foundUser.Firstname + " " + foundUser.Lastname) : null, null);

                Create(new SmsSendingQueue()
                {
                    Body = smsMessage,
                    CreateDate = DateTime.Now,
                    MobileNumber = input.username,
                    SiteSettingId = siteSettingId.ToIntReturnZiro(),
                    Subject = curType.GetEnumDisplayName(),
                    Type = curType
                }, siteSettingId, GlobalConfig.GetSmsLimitFromConfig(), null);
                SaveChange();

                return ApiResult.GenerateNewResult(true, BMessages.Please_Enter_SMSCode, new
                {
                    data = new { username = input.username },
                    labels = new List<object>() { new { inputName = "code", labelText = "کد  به شماره  " + input.username + " ارسال گردید" } },
                    stepId = "recoveryPasswordConfirmSMS",
                    countDownId = "tryAginButtonCDRP"
                });
            }

            return ApiResult.GenerateNewResult(true, BMessages.Please_Enter_SMSCode, new
            {
                data = new { username = input.username },
                labels = new List<object>() { new { inputName = "code", labelText = "کد  به شماره  " + input.username + " ارسال گردید" } },
                stepId = "recoveryPasswordConfirmSMS",
                countDownId = "tryAginButtonCDRP"
            });
        }

        private void ActiveCodeForResetPasswordValidation(RegLogSMSVM input, IpSections ipSections, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (string.IsNullOrEmpty(input.username))
                throw BException.GenerateNewException(BMessages.Please_Enter_Mobile);
            if (!input.username.IsMobile())
                throw BException.GenerateNewException(BMessages.Invalid_Mobile_Number);
            if (ipSections == null)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            
        }
    }
}
