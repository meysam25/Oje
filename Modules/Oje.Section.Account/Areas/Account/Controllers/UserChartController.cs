using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Interfaces;

namespace Oje.Section.Account.Areas.Account.Controllers
{
    [Area("Account")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = " هویتی (ادمین)", Icon = "fa-users", Title = "شمای کلی کاربران")]
    [CustomeAuthorizeFilter]
    public class UserChartController: Controller
    {
        readonly IUserService UserService = null;
        readonly ISiteSettingService SiteSettingService = null;

        public UserChartController
            (
                IUserService UserService,
                ISiteSettingService SiteSettingService
            )
        {
            this.UserService = UserService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "شمای کلی کاربران", Icon = "fa-file-powerpoint", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "شمای کلی کاربران";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "UserChart", new { area = "Account" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه شمای کلی کاربران", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Account", "UserChart")));
        }

        [AreaConfig(Title = "شمای کلی کاربران", Icon = "fa-list-alt")]
        [HttpPost]
        public IActionResult GetSource()
        {
            return Json(UserService.GetUserChart(SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
