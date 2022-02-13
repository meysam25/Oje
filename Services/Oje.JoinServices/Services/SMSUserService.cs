using Microsoft.AspNetCore.Http;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.JoinServices.Interfaces;
using Oje.Sms.Interfaces;
using Oje.Sms.Models.DB;
using Oje.Sms.Models.View;

namespace Oje.JoinServices.Services
{
    public class SMSUserService : ISMSUserService
    {
        readonly ISmsValidationHistoryService SmsValidationHistoryService = null;
        readonly AccountService.Interfaces.IUserService UserService = null;
        readonly AccountService.Interfaces.IRoleService RoleService = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;
        readonly ISmsSendingQueueService SmsSendingQueueService = null;
        readonly ISmsTemplateService SmsTemplateService = null;
        readonly IHttpContextAccessor HttpContextAccessor = null;

        public SMSUserService
            (
                ISmsValidationHistoryService SmsValidationHistoryService,
                AccountService.Interfaces.IUserService UserService,
                AccountService.Interfaces.IRoleService RoleService,
                AccountService.Interfaces.ISiteSettingService SiteSettingService,
                ISmsSendingQueueService SmsSendingQueueService,
                ISmsTemplateService SmsTemplateService,
                IHttpContextAccessor HttpContextAccessor
            )
        {
            this.SmsValidationHistoryService = SmsValidationHistoryService;
            this.UserService = UserService;
            this.RoleService = RoleService;
            this.SiteSettingService = SiteSettingService;
            this.SmsSendingQueueService = SmsSendingQueueService;
            this.SmsTemplateService = SmsTemplateService;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public object ChagePasswordAndLogin(ChangePasswordAndLoginVM input, IpSections ipSections, int? siteSettingId)
        {
            ChagePasswordAndLoginValidation(input, ipSections, siteSettingId);

            var foundUser = UserService.GetBy(input.username, siteSettingId.Value);
            if (foundUser == null || foundUser.IsActive == false || foundUser.IsDelete == true)
                throw BException.GenerateNewException(BMessages.Validation_Error);

            bool isValidPreUsed = SmsValidationHistoryService.IsValidPreUsed(input.username.ToLongReturnZiro(), input.codeId.Decrypt2(), ipSections);

            if (isValidPreUsed == false)
                throw BException.GenerateNewException(BMessages.Invalid_Code);

            UserService.UpdatePassword(foundUser, input.password);
            UserService.setCookieForThisUser(foundUser, new AccountService.Models.View.LoginVM() { rememberMe = true });

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull, new { stepId = "rigLogStep", hideModal = true, userfullname = (string.IsNullOrEmpty(foundUser.Firstname) ? foundUser.Username : (foundUser.Firstname + " " + foundUser.Lastname)) });
        }

        private void ChagePasswordAndLoginValidation(ChangePasswordAndLoginVM input, IpSections ipSections, int? siteSettingId)
        {
            if (input == null)
                throw BException.GenerateNewException(BMessages.Please_Fill_All_Parameters);
            if (ipSections == null)
                throw BException.GenerateNewException(BMessages.Validation_Error);
            if (siteSettingId.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.SiteSetting_Can_Not_Be_Founded);
            if (string.IsNullOrEmpty(input.username))
                throw BException.GenerateNewException(BMessages.Please_Enter_Username);
            if (!input.username.IsMobile())
                throw BException.GenerateNewException(BMessages.Invalid_Mobile_Number);
            if (string.IsNullOrEmpty(input.codeId))
                throw BException.GenerateNewException(BMessages.Please_Enter_Code);
            try { input.codeId.Decrypt2(); } catch { throw BException.GenerateNewException(BMessages.Invalid_Code); }
            if (string.IsNullOrEmpty(input.password))
                throw BException.GenerateNewException(BMessages.Please_Enter_Password);
            if (string.IsNullOrEmpty(input.confirmPassword))
                throw BException.GenerateNewException(BMessages.Please_Enter_Password);
            if (input.password != input.confirmPassword)
                throw BException.GenerateNewException(BMessages.The_Password_Is_Not_Look_Like_Confirm_Password);
            if (input.password.Length > 30)
                throw BException.GenerateNewException(BMessages.Password_Can_Not_Be_More_Then_30_Chars);
            if (input.password.Length < 6)
                throw BException.GenerateNewException(BMessages.Password_Can_Not_Be_Less_Then_6_Chars);
            if (input.password.IsWeekPassword())
                throw BException.GenerateNewException(BMessages.The_Password_Is_Week);
        }

        public object CheckIfSmsCodeIsValid(RegLogSMSVM input, IpSections ipSections, int? siteSettingId)
        {
            LoginRegisterValidation(input, ipSections, siteSettingId);

            var foundUser = UserService.GetBy(input.username, siteSettingId);

            if (foundUser != null)
            {
                if (foundUser != null && (foundUser.IsDelete == true || foundUser.IsActive == false))
                    throw BException.GenerateNewException(BMessages.Validation_Error);

                string codeId = SmsValidationHistoryService.ValidatePreUsedBy(input.username.ToLongReturnZiro(), input.code.ToIntReturnZiro(), ipSections);
                if (string.IsNullOrEmpty(codeId))
                    throw BException.GenerateNewException(BMessages.Invalid_Code);

                return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull, new { stepId = "recoveryPasswordChangePassword", data = new { username = input.username, codeId = codeId } });
            }

            throw BException.GenerateNewException(BMessages.Invalid_Code);
        }

        public object LoginRegister(RegLogSMSVM input, IpSections ipSections, int? siteSettingId)
        {
            LoginRegisterValidation(input, ipSections, siteSettingId);

            var foundUser = UserService.GetBy(input.username, siteSettingId);

            if (foundUser != null && (foundUser.IsDelete == true || foundUser.IsActive == false))
                throw BException.GenerateNewException(BMessages.Validation_Error);

            bool isValid = SmsValidationHistoryService.ValidateBy(input.username.ToLongReturnZiro(), input.code.ToIntReturnZiro(), ipSections);
            if (!isValid)
                throw BException.GenerateNewException(BMessages.Invalid_Code);

            if (foundUser != null)
                UserService.setCookieForThisUser(foundUser, new AccountService.Models.View.LoginVM() { rememberMe = true });
            else
            {
                var foundUserRole = RoleService.CreateGet("user", "کاربر", 1, RoleType.User);
                if (foundUserRole == null)
                    throw BException.GenerateNewException(BMessages.Validation_Error);
                string password = RandomService.GeneratePassword(12);
                UserService.Create(new AccountService.Models.View.CreateUpdateUserVM()
                {
                    username = input.username,
                    firstname = " ",
                    lastname = " ",
                    password = password,
                    confirmPassword = password,
                    isActive = true,
                    isMobileConfirm = true,
                    mobile = input.username,
                    sitesettingId = siteSettingId,
                    roleIds = new List<int>() { foundUserRole.Id },
                    email = input.username + "@" + HttpContextAccessor.HttpContext.Request.Host
                }, SiteSettingService.GetSiteSetting()?.UserId);
                foundUser = UserService.GetBy(input.username, siteSettingId);
                if (foundUser == null)
                    throw BException.GenerateNewException(BMessages.UnknownError);
                UserService.setCookieForThisUser(foundUser, new AccountService.Models.View.LoginVM() { rememberMe = true });

                List<SmsTemplate> foundTemplate = null;
                string smsMessage = "کاربر گرامی ثبت نام شما با موفقیت انجام گرفت کلمه عبور شما عبارت است از " + Environment.NewLine + password;

                foundTemplate = SmsTemplateService.GetBy(UserNotificationType.RegisterSuccessFull, siteSettingId);
                if (foundTemplate != null && foundTemplate.Count > 0)
                    smsMessage = GlobalServices.replaceKeyword(foundTemplate.Select(t => t.Description).FirstOrDefault(), null, password, foundUser != null ? (foundUser.Username) : null);

                SmsSendingQueueService.Create(new SmsSendingQueue()
                {
                    Body = smsMessage,
                    CreateDate = DateTime.Now,
                    Ip1 = ipSections.Ip1,
                    Ip2 = ipSections.Ip2,
                    Ip3 = ipSections.Ip3,
                    Ip4 = ipSections.Ip4,
                    MobileNumber = foundUser.Username,
                    SiteSettingId = siteSettingId.ToIntReturnZiro(),
                    Subject = UserNotificationType.RegisterSuccessFull.GetEnumDisplayName(),
                    Type = UserNotificationType.RegisterSuccessFull
                }, siteSettingId, GlobalConfig.GetSmsLimitFromConfig(), null);
                SmsSendingQueueService.SaveChange();
            }

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull, new { stepId = "rigLogStep", hideModal = true, userfullname = input.username });
        }

        private void LoginRegisterValidation(RegLogSMSVM input, IpSections ipSections, int? siteSettingId)
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
            if (string.IsNullOrEmpty(input.code))
                throw BException.GenerateNewException(BMessages.Please_Enter_Code);
            if (input.code.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Just_Use_Number_For_Code);
            if (input.code.Length > 20)
                throw BException.GenerateNewException(BMessages.Validation_Error);
        }
    }
}
