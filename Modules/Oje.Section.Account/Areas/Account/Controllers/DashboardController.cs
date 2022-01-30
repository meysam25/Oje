using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Models.View;
using Oje.Sms.Models.View;
using Oje.JoinServices.Interfaces;
using Oje.Security.Interfaces;

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

        public DashboardController(
            IUserService UserService,
            ISiteSettingService SiteSettingService,
            ISectionService SectionService,
            IDashboardSectionService DashboardSectionService,
            Sms.Interfaces.ISmsSendingQueueService SmsSendingQueueService,
            ISMSUserService SMSUserService,
            IBlockAutoIpService BlockAutoIpService
            )
        {
            this.UserService = UserService;
            this.SiteSettingService = SiteSettingService;
            this.SectionService = SectionService;
            this.DashboardSectionService = DashboardSectionService;
            this.SmsSendingQueueService = SmsSendingQueueService;
            this.SMSUserService = SMSUserService;
            this.BlockAutoIpService = BlockAutoIpService;
        }

        [CustomeAuthorizeFilter]
        [AreaConfig(Title = "داشبرد", Icon = "fa-home", IsMainMenuItem = false)]
        public IActionResult Index()
        {
            ViewBag.Title = "داشبرد";
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
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm] LoginVM input)
        {
            BlockAutoIpService.CheckIfRequestIsValid(Infrastructure.Enums.BlockClientConfigType.LoginWithPassword, Infrastructure.Enums.BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            var tempResult = UserService.Login(input, SiteSettingService.GetSiteSetting()?.Id);
            BlockAutoIpService.CheckIfRequestIsValid(Infrastructure.Enums.BlockClientConfigType.LoginWithPassword, Infrastructure.Enums.BlockAutoIpAction.AfterExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            return Json(tempResult);
        }

        public IActionResult Logout()
        {
            UserService.Logout(HttpContext.GetLoginUser());
            try
            {
                string referer = Request.Headers["Referer"].ToString();
                ViewBag.referer = referer;
            }
            catch { }
            ViewBag.loginUrl = Url.Action("Login");
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
            return Json(SectionService.GetSideMenuAjax(HttpContext.GetLoginUser()?.UserId));
        }

        [HttpPost]
        public IActionResult IsUserLogin()
        {
            return Json(HttpContext.GetLoginUser()?.UserId > 0);
        }

        [HttpPost]
        public IActionResult LoginWithSMS(RegLogSMSVM input)
        {
            BlockAutoIpService.CheckIfRequestIsValid(Infrastructure.Enums.BlockClientConfigType.LoginWithSMS, Infrastructure.Enums.BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            var tempResult = SmsSendingQueueService.LoginWithSMS(input, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            BlockAutoIpService.CheckIfRequestIsValid(Infrastructure.Enums.BlockClientConfigType.LoginWithSMS, Infrastructure.Enums.BlockAutoIpAction.AfterExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            return Json(tempResult);
        }

        [HttpPost]
        public IActionResult LoginRegister(RegLogSMSVM input)
        {
            return Json(SMSUserService.LoginRegister(input, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id));
        }

        [HttpPost]
        public IActionResult ActiveCodeForResetPassword(RegLogSMSVM input)
        {
            BlockAutoIpService.CheckIfRequestIsValid(Infrastructure.Enums.BlockClientConfigType.ActiveCodeForResetPassword, Infrastructure.Enums.BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            var tempResult = SmsSendingQueueService.ActiveCodeForResetPassword(input, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            BlockAutoIpService.CheckIfRequestIsValid(Infrastructure.Enums.BlockClientConfigType.ActiveCodeForResetPassword, Infrastructure.Enums.BlockAutoIpAction.AfterExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            return Json(tempResult);
        }

        [HttpPost]
        public IActionResult CheckIfSmsCodeIsValid(RegLogSMSVM input)
        {
            BlockAutoIpService.CheckIfRequestIsValid(Infrastructure.Enums.BlockClientConfigType.CheckIfSmsCodeIsValid, Infrastructure.Enums.BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            var tempResult = SMSUserService.CheckIfSmsCodeIsValid(input, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            BlockAutoIpService.CheckIfRequestIsValid(Infrastructure.Enums.BlockClientConfigType.CheckIfSmsCodeIsValid, Infrastructure.Enums.BlockAutoIpAction.AfterExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            return Json(tempResult);
        }

        [HttpPost]
        public IActionResult ChangePasswordAndLogin(ChangePasswordAndLoginVM input)
        {
            BlockAutoIpService.CheckIfRequestIsValid(Infrastructure.Enums.BlockClientConfigType.ChangePasswordAndLogin, Infrastructure.Enums.BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            var tempResult = SMSUserService.ChagePasswordAndLogin(input, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            BlockAutoIpService.CheckIfRequestIsValid(Infrastructure.Enums.BlockClientConfigType.ChangePasswordAndLogin, Infrastructure.Enums.BlockAutoIpAction.AfterExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            return Json(tempResult);
        }
    }
}
