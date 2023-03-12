using Microsoft.AspNetCore.Http;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.JoinServices.Interfaces;
using Oje.Security.Interfaces;
using Oje.Sms.Interfaces;
using Oje.Sms.Models.DB;
using Oje.Sms.Models.View;
using IBlockLoginUserService = Oje.Sms.Interfaces.IBlockLoginUserService;

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
        readonly IUserLoginLogoutLogService UserLoginLogoutLogService = null;
        readonly IBlockLoginUserService BlockLoginUserService = null;

        public SMSUserService
            (
                ISmsValidationHistoryService SmsValidationHistoryService,
                AccountService.Interfaces.IUserService UserService,
                AccountService.Interfaces.IRoleService RoleService,
                AccountService.Interfaces.ISiteSettingService SiteSettingService,
                ISmsSendingQueueService SmsSendingQueueService,
                ISmsTemplateService SmsTemplateService,
                IHttpContextAccessor HttpContextAccessor,
                IUserLoginLogoutLogService UserLoginLogoutLogService,
                IBlockLoginUserService BlockLoginUserService
            )
        {
            this.SmsValidationHistoryService = SmsValidationHistoryService;
            this.UserService = UserService;
            this.RoleService = RoleService;
            this.SiteSettingService = SiteSettingService;
            this.SmsSendingQueueService = SmsSendingQueueService;
            this.SmsTemplateService = SmsTemplateService;
            this.HttpContextAccessor = HttpContextAccessor;
            this.UserLoginLogoutLogService = UserLoginLogoutLogService;
            this.BlockLoginUserService = BlockLoginUserService;
        }

        public object ChagePasswordAndLogin(ChangePasswordAndLoginVM input, IpSections ipSections, int? siteSettingId)
        {
            ChagePasswordAndLoginValidation(input, ipSections, siteSettingId);

            if (!BlockLoginUserService.IsValidDay(DateTime.Now, siteSettingId))
                throw BException.GenerateNewException(BMessages.UnknownError);

            var foundUser = UserService.GetBy(input.username, siteSettingId.Value);
            if (foundUser == null || foundUser.IsActive == false || foundUser.IsDelete == true)
                throw BException.GenerateNewException(BMessages.UnknownError, ApiResultErrorCode.ValidationError, foundUser?.Id ?? 0);
            if (!foundUser.IsSignature())
                throw BException.GenerateNewException(BMessages.UnknownError);
            if (foundUser.UserRoles != null)
                foreach (var role in foundUser.UserRoles)
                    if (!role.IsSignature())
                        throw BException.GenerateNewException(BMessages.UnknownError);

            if (string.IsNullOrEmpty(input.codeId))
                throw BException.GenerateNewException(BMessages.Please_Enter_Code, ApiResultErrorCode.ValidationError, foundUser?.Id ?? 0);
            try { input.codeId.Decrypt2(); } catch { throw BException.GenerateNewException(BMessages.Invalid_Code, ApiResultErrorCode.ValidationError, foundUser?.Id ?? 0); }
            if (string.IsNullOrEmpty(input.password))
                throw BException.GenerateNewException(BMessages.Please_Enter_Password, ApiResultErrorCode.ValidationError, foundUser?.Id ?? 0);
            if (string.IsNullOrEmpty(input.confirmPassword))
                throw BException.GenerateNewException(BMessages.Please_Enter_Password, ApiResultErrorCode.ValidationError, foundUser?.Id ?? 0);
            if (input.password != input.confirmPassword)
                throw BException.GenerateNewException(BMessages.The_Password_Is_Not_Look_Like_Confirm_Password, ApiResultErrorCode.ValidationError, foundUser?.Id ?? 0);
            if (input.password.Length > 30)
                throw BException.GenerateNewException(BMessages.Password_Can_Not_Be_More_Then_30_Chars, ApiResultErrorCode.ValidationError, foundUser?.Id ?? 0);
            if (input.password.Length < 6)
                throw BException.GenerateNewException(BMessages.Password_Can_Not_Be_Less_Then_6_Chars, ApiResultErrorCode.ValidationError, foundUser?.Id ?? 0);
            if (input.password.IsWeekPassword())
                throw BException.GenerateNewException(BMessages.The_Password_Is_Week, ApiResultErrorCode.ValidationError, foundUser?.Id ?? 0);

            bool isValidPreUsed = SmsValidationHistoryService.IsValidPreUsed(input.username.ToLongReturnZiro(), input.codeId.Decrypt2(), ipSections);

            if (isValidPreUsed == false)
                throw BException.GenerateNewException(BMessages.Invalid_Code, ApiResultErrorCode.ValidationError, foundUser?.Id ?? 0);

            UserService.UpdatePassword(foundUser, input.password);
            UserService.setCookieForThisUser(foundUser, new AccountService.Models.View.LoginVM() { rememberMe = true }, RoleService.HasAnyAutoRefreshRole(foundUser.Id), RoleService.HasAnySeeOtherSiteRoleConfig(foundUser.Id));
            UserService.UpdateUserSessionFileName(foundUser?.Id, foundUser.tempLastSession);

            UserLoginLogoutLogService.Create(foundUser.Id, UserLoginLogoutLogType.LoginWithChangePassword, SiteSettingService.GetSiteSetting()?.Id, true, BMessages.Operation_Was_Successfull.GetEnumDisplayName());

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull, new 
            { 
                stepId = "rigLogStep", 
                hideModal = true, 
                userfullname = (string.IsNullOrEmpty(foundUser.Firstname) ? foundUser.Username : (foundUser.Firstname + " " + foundUser.Lastname)) ,
                isUser = UserService.isWebsiteUser(foundUser.Id)
            });
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

        }

        public object CheckIfSmsCodeIsValid(RegLogSMSVM input, IpSections ipSections, int? siteSettingId)
        {
            LoginRegisterValidation(input, ipSections, siteSettingId);

            if (!BlockLoginUserService.IsValidDay(DateTime.Now, siteSettingId))
                throw BException.GenerateNewException(BMessages.UnknownError);

            var foundUser = UserService.GetBy(input.username, siteSettingId);

            if (string.IsNullOrEmpty(input.code))
                throw BException.GenerateNewException(BMessages.Please_Enter_Code, ApiResultErrorCode.ValidationError, foundUser?.Id ?? 0);
            if (input.code.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Just_Use_Number_For_Code, ApiResultErrorCode.ValidationError, foundUser?.Id ?? 0);
            if (input.code.Length > 20)
                throw BException.GenerateNewException(BMessages.Validation_Error, ApiResultErrorCode.ValidationError, foundUser?.Id ?? 0);

            if (foundUser != null)
            {
                if (!foundUser.IsSignature())
                    throw BException.GenerateNewException(BMessages.UnknownError);
                if (foundUser.UserRoles != null)
                    foreach (var role in foundUser.UserRoles)
                        if (!role.IsSignature())
                            throw BException.GenerateNewException(BMessages.UnknownError);
                if (foundUser != null && (foundUser.IsDelete == true || foundUser.IsActive == false))
                    throw BException.GenerateNewException(BMessages.Validation_Error, ApiResultErrorCode.ValidationError, foundUser.Id);

                string codeId = SmsValidationHistoryService.ValidatePreUsedBy(input.username.ToLongReturnZiro(), input.code.ToIntReturnZiro(), ipSections);
                if (string.IsNullOrEmpty(codeId))
                    throw BException.GenerateNewException(BMessages.Invalid_Code, ApiResultErrorCode.ValidationError, foundUser.Id);

                return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull, new { stepId = "recoveryPasswordChangePassword", data = new { username = input.username, codeId = codeId } });
            }

            throw BException.GenerateNewException(BMessages.Invalid_Code);
        }

        public object LoginRegister(RegLogSMSVM input, IpSections ipSections, int? siteSettingId)
        {
            LoginRegisterValidation(input, ipSections, siteSettingId);

            if (!BlockLoginUserService.IsValidDay(DateTime.Now, siteSettingId))
                throw BException.GenerateNewException(BMessages.UnknownError);

            var foundUser = UserService.GetBy(input.username, siteSettingId);

            if (foundUser != null && (foundUser.IsDelete == true || foundUser.IsActive == false))
                throw BException.GenerateNewException(BMessages.Validation_Error);

            if (string.IsNullOrEmpty(input.code))
                throw BException.GenerateNewException(BMessages.Please_Enter_Code, ApiResultErrorCode.ValidationError, foundUser?.Id ?? 0);
            if (input.code.ToIntReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Please_Just_Use_Number_For_Code, ApiResultErrorCode.ValidationError, foundUser?.Id ?? 0);
            if (input.code.Length > 20)
                throw BException.GenerateNewException(BMessages.Validation_Error, ApiResultErrorCode.ValidationError, foundUser?.Id ?? 0);
            bool isValid = SmsValidationHistoryService.ValidateBy(input.username.ToLongReturnZiro(), input.code.ToIntReturnZiro(), ipSections);
            if (!isValid)
                throw BException.GenerateNewException(BMessages.Invalid_Code, ApiResultErrorCode.ValidationError, foundUser?.Id ?? 0);

            if (foundUser != null)
            {
                if (!foundUser.IsSignature())
                    throw BException.GenerateNewException(BMessages.UnknownError);
                if (foundUser.UserRoles != null)
                    foreach (var role in foundUser.UserRoles)
                        if (!role.IsSignature())
                            throw BException.GenerateNewException(BMessages.UnknownError);

                UserService.setCookieForThisUser(foundUser, new AccountService.Models.View.LoginVM() { rememberMe = true }, RoleService.HasAnyAutoRefreshRole(foundUser.Id), RoleService.HasAnySeeOtherSiteRoleConfig(foundUser.Id));
                UserService.UpdateUserSessionFileName(foundUser?.Id, foundUser.tempLastSession);
                UserLoginLogoutLogService.Create(foundUser.Id, UserLoginLogoutLogType.LoginWithPhoneNumber, SiteSettingService.GetSiteSetting()?.Id, true, BMessages.Operation_Was_Successfull.GetEnumDisplayName());
            }
            else
            {
                var foundUserRole = RoleService.CreateGet("user", "کاربر", 1, RoleType.User);
                if (foundUserRole == null)
                    throw BException.GenerateNewException(BMessages.Validation_Error, ApiResultErrorCode.ValidationError, foundUser?.Id ?? 0);
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
                    throw BException.GenerateNewException(BMessages.UnknownError, ApiResultErrorCode.ValidationError, foundUser?.Id ?? 0);
                UserService.setCookieForThisUser(foundUser, new AccountService.Models.View.LoginVM() { rememberMe = true }, RoleService.HasAnyAutoRefreshRole(foundUser.Id), RoleService.HasAnySeeOtherSiteRoleConfig(foundUser.Id));
                UserService.UpdateUserSessionFileName(foundUser?.Id, foundUser.tempLastSession);

                List<SmsTemplate> foundTemplate = null;
                string smsMessage = "کاربر گرامی ثبت نام شما با موفقیت انجام گرفت کلمه عبور شما عبارت است از " + Environment.NewLine + password;

                foundTemplate = SmsTemplateService.GetBy(UserNotificationType.RegisterSuccessFull, siteSettingId);
                if (foundTemplate != null && foundTemplate.Count > 0)
                    smsMessage = GlobalServices.replaceKeyword(foundTemplate.Select(t => t.Description).FirstOrDefault(), null, password, foundUser != null ? (foundUser.Username) : null, null);

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
                UserLoginLogoutLogService.Create(foundUser.Id, UserLoginLogoutLogType.LoginWithPhoneNumber, SiteSettingService.GetSiteSetting()?.Id, true, BMessages.Operation_Was_Successfull.GetEnumDisplayName());
            }

            return ApiResult.GenerateNewResult(true, BMessages.Operation_Was_Successfull, new 
            { 
                stepId = "rigLogStep", 
                hideModal = true, 
                userfullname = (!string.IsNullOrEmpty(foundUser.Firstname) ? (foundUser.Firstname + " " + foundUser.Lastname) : foundUser.Username),
                isUser = UserService.isWebsiteUser(foundUser.Id) 
            });
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
        }
    }
}
