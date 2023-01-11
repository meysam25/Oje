using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Models.View;
using Oje.Sms.Models.View;
using Oje.JoinServices.Interfaces;
using Oje.Security.Interfaces;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure;

namespace Oje.Section.Account.Areas.Account.Controllers
{
    [Area("Account")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = " هویتی (ادمین)", Icon = "fa-users", Title = "داشبرد")]
    public class DashboardController : Controller
    {
        readonly IUserService UserService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly ISectionService SectionService = null;
        readonly IDashboardSectionService DashboardSectionService = null;
        readonly Sms.Interfaces.ISmsSendingQueueService SmsSendingQueueService = null;
        readonly ISMSUserService SMSUserService = null;
        readonly IBlockAutoIpService BlockAutoIpService = null;
        readonly IUserLoginLogoutLogService UserLoginLogoutLogService = null;
        readonly ILoginDescrptionService LoginDescrptionService = null;
        readonly ILoginBackgroundImageService LoginBackgroundImageService = null;

        public DashboardController(
                IUserService UserService,
                ISiteSettingService SiteSettingService,
                ISectionService SectionService,
                IDashboardSectionService DashboardSectionService,
                Sms.Interfaces.ISmsSendingQueueService SmsSendingQueueService,
                ISMSUserService SMSUserService,
                IBlockAutoIpService BlockAutoIpService,
                IUserLoginLogoutLogService UserLoginLogoutLogService,
                ILoginDescrptionService LoginDescrptionService,
                ILoginBackgroundImageService LoginBackgroundImageService
            )
        {
            this.UserService = UserService;
            this.SiteSettingService = SiteSettingService;
            this.SectionService = SectionService;
            this.DashboardSectionService = DashboardSectionService;
            this.SmsSendingQueueService = SmsSendingQueueService;
            this.SMSUserService = SMSUserService;
            this.BlockAutoIpService = BlockAutoIpService;
            this.UserLoginLogoutLogService = UserLoginLogoutLogService;
            this.LoginDescrptionService = LoginDescrptionService;
            this.LoginBackgroundImageService = LoginBackgroundImageService;
        }

        [CustomeAuthorizeFilter]
        [AreaConfig(Title = "داشبورد", Icon = "fa-home", IsMainMenuItem = false)]
        public IActionResult Index()
        {
            ViewBag.Title = "داشبورد";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "Dashboard", new { area = "Account" });
            return View("Index");
        }

        [CustomeAuthorizeFilter]
        [AreaConfig(Title = "تنظیمات صفحه داشبورد", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(DashboardSectionService.GetDashboardConfigByUserId(SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId));
        }

        [HttpGet]
        public IActionResult Login()
        {
            var curSetting = SiteSettingService.GetSiteSetting();
            if (curSetting == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            GlobalServices.FillSeoInfo(
                  ViewData,
                   curSetting.Title,
                   curSetting.SeoMainPage,
                   Request.Scheme + "://" + Request.Host + "/Account/Dashboard/Login",
                   Request.Scheme + "://" + Request.Host + "/Account/Dashboard/Login",
                   WebSiteTypes.website,
                   Request.Scheme + "://" + Request.Host + GlobalConfig.FileAccessHandlerUrl + curSetting.Image512,
                   null
                   );

            ViewBag.HideLoginButton = true;
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm] LoginVM input)
        {
            object tempResult = null;
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.LoginWithPassword, BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            try { tempResult = UserService.Login(input, SiteSettingService.GetSiteSetting()?.Id); } catch (BException ex) { UserLoginLogoutLogService.Create(ex.UserId, UserLoginLogoutLogType.LoginWithPassword, SiteSettingService.GetSiteSetting()?.Id, false, ex.Message); throw; } catch { throw; }
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.LoginWithPassword, BlockAutoIpAction.AfterExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);

            return Json(tempResult);
        }

        public IActionResult Logout()
        {
            var curUser = HttpContext.GetLoginUser();
            UserService.Logout(curUser);
            try
            {
                string referer = Request.Headers["Referer"].ToString();
                ViewBag.referer = referer;
            }
            catch { }
            ViewBag.loginUrl = Url.Action("Login");
            UserLoginLogoutLogService.Create(curUser?.UserId ?? 0, UserLoginLogoutLogType.Logout, SiteSettingService.GetSiteSetting()?.Id, true, BMessages.Operation_Was_Successfull.GetEnumDisplayName());

            if (Request.Headers.ContainsKey("X-Requested-With"))
                return Json(new { isSuccess = true, message = BMessages.Operation_Was_Successfull.GetEnumDisplayName() });

            return View();
        }

        [AreaConfig(Title = "مشاهده مشخصات کاربر لاگین شده")]
        [HttpPost]
        public IActionResult GetLoginUserInfo()
        {
            return Json(UserService.GetUserInfoByUserId(HttpContext.GetLoginUser()?.UserId));
        }

        [AreaConfig(Title = "مشاهده منو های کاربر")]
        [HttpPost]
        public IActionResult GetLoginUserSideMenu()
        {
            return Json(SectionService.GetSideMenuWidthCategory(HttpContext.GetLoginUser()?.UserId));
        }

        [HttpPost]
        public IActionResult IsUserLogin()
        {
            return Json(HttpContext.GetLoginUser()?.UserId > 0);
        }

        [HttpPost]
        public IActionResult LoginWithSMS(RegLogSMSVM input)
        {
            object tempResult = null;
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.LoginWithSMS, BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            try { tempResult = SmsSendingQueueService.LoginWithSMS(input, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id); } catch (BException ex) { UserLoginLogoutLogService.Create(ex.UserId, UserLoginLogoutLogType.LoginWithPhoneNumber, SiteSettingService.GetSiteSetting()?.Id, false, ex.Message); throw; } catch { throw; }
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.LoginWithSMS, BlockAutoIpAction.AfterExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            return Json(tempResult);
        }

        [HttpPost]
        public IActionResult LoginRegister(RegLogSMSVM input)
        {
            object tempResult = null;
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.LoginRegister, BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            try { tempResult = SMSUserService.LoginRegister(input, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id); } catch (BException ex) { UserLoginLogoutLogService.Create(ex.UserId, UserLoginLogoutLogType.LoginWithPhoneNumber, SiteSettingService.GetSiteSetting()?.Id, false, ex.Message); throw; } catch { throw; }
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.LoginRegister, BlockAutoIpAction.AfterExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            return Json(tempResult);
        }

        [HttpPost]
        public IActionResult ActiveCodeForResetPassword(RegLogSMSVM input)
        {
            object tempResult = null;
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.ActiveCodeForResetPassword, BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            try { tempResult = SmsSendingQueueService.ActiveCodeForResetPassword(input, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id); } catch (BException ex) { UserLoginLogoutLogService.Create(ex.UserId, UserLoginLogoutLogType.LoginWithChangePassword, SiteSettingService.GetSiteSetting()?.Id, false, ex.Message); throw; } catch { throw; }
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.ActiveCodeForResetPassword, BlockAutoIpAction.AfterExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            return Json(tempResult);
        }

        [HttpPost]
        public IActionResult CheckIfSmsCodeIsValid(RegLogSMSVM input)
        {
            object tempResult = null;
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.CheckIfSmsCodeIsValid, BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            try { tempResult = SMSUserService.CheckIfSmsCodeIsValid(input, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id); } catch (BException ex) { UserLoginLogoutLogService.Create(ex.UserId, UserLoginLogoutLogType.LoginWithPhoneNumber, SiteSettingService.GetSiteSetting()?.Id, false, ex.Message); throw; } catch { throw; }
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.CheckIfSmsCodeIsValid, BlockAutoIpAction.AfterExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            return Json(tempResult);
        }

        [HttpPost]
        public IActionResult ChangePasswordAndLogin(ChangePasswordAndLoginVM input)
        {
            object tempResult = null;
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.ChangePasswordAndLogin, BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            try { tempResult = SMSUserService.ChagePasswordAndLogin(input, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id); } catch (BException ex) { UserLoginLogoutLogService.Create(ex.UserId, UserLoginLogoutLogType.LoginWithChangePassword, SiteSettingService.GetSiteSetting()?.Id, false, ex.Message); throw; } catch { throw; }
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.ChangePasswordAndLogin, BlockAutoIpAction.AfterExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            return Json(tempResult);
        }

        [HttpPost]
        public IActionResult GetLoginDescription([FromForm] string returnUrl)
        {
            return Json(LoginDescrptionService.GetBy(SiteSettingService.GetSiteSetting()?.Id, returnUrl));
        }

        [HttpPost]
        public IActionResult GetLoginBackgroundImage()
        {
            return Json(LoginBackgroundImageService.GetRandom(SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
