using Oje.AccountManager.Filters;
using Oje.AccountManager.Interfaces;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Oje.AccountManager.Models.View;

namespace Oje.Section.Account.Areas.Account.Controllers
{
    [Area("Account")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = " هویتی (ادمین)", Icon = "fa-users", Title = "داشبرد")]
    public class DashboardController : Controller
    {
        readonly IUserManager UserManager = null;
        readonly ISiteSettingManager SiteSettingManager = null;
        readonly ISectionManager SectionManager = null;
        public DashboardController(
            IUserManager UserManager,
            ISiteSettingManager SiteSettingManager,
            ISectionManager SectionManager
            )
        {
            this.UserManager = UserManager;
            this.SiteSettingManager = SiteSettingManager;
            this.SectionManager = SectionManager;
        }

        [CustomeAuthorizeFilter]
        [AreaConfig(Title = "داشبرد", Icon = "fa-home", IsMainMenuItem = false)]
        public IActionResult Index()
        {
            ViewBag.Title = "داشبرد";
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm]LoginVM input)
        {
            return Json(UserManager.Login(input, SiteSettingManager.GetSiteSetting()?.Id));
        }

        public IActionResult Logout()
        {
            UserManager.Logout();
            return RedirectToAction("Login");
        }

        [AreaConfig(Title = "مشاهده مشخصات کاربر لاگین شده")]
        [CustomeAuthorizeFilter]
        [HttpPost]
        public IActionResult GetLoginUserInfo()
        {
            return Json(UserManager.GetUserInfoByUserId(HttpContext.GetLoginUserId()?.UserId));
        }

        [AreaConfig(Title = "مشاهده منو های کاربر")]
        [CustomeAuthorizeFilter]
        [HttpPost]
        public IActionResult GetLoginUserSideMenu()
        {
            return Json(SectionManager.GetSideMenuAjax(HttpContext.GetLoginUserId()?.UserId));
        }
    }
}
