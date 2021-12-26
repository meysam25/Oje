using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Models.View;
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

        public DashboardController(
            IUserService UserService,
            ISiteSettingService SiteSettingService,
            ISectionService SectionService,
            IDashboardSectionService DashboardSectionService
            )
        {
            this.UserService = UserService;
            this.SiteSettingService = SiteSettingService;
            this.SectionService = SectionService;
            this.DashboardSectionService = DashboardSectionService;
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
            return Content(DashboardSectionService.GetDashboardConfigByUserId(SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm] LoginVM input)
        {
            return Json(UserService.Login(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        public IActionResult Logout()
        {
            UserService.Logout();
            return RedirectToAction("Login");
        }

        [AreaConfig(Title = "مشاهده مشخصات کاربر لاگین شده")]
        [CustomeAuthorizeFilter]
        [HttpPost]
        public IActionResult GetLoginUserInfo()
        {
            return Json(UserService.GetUserInfoByUserId(HttpContext.GetLoginUserId()?.UserId));
        }

        [AreaConfig(Title = "مشاهده منو های کاربر")]
        [CustomeAuthorizeFilter]
        [HttpPost]
        public IActionResult GetLoginUserSideMenu()
        {
            return Json(SectionService.GetSideMenuAjax(HttpContext.GetLoginUserId()?.UserId));
        }
    }
}
