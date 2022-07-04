using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Security.Interfaces;
using Oje.Security.Models.View;

namespace Oje.Section.Security.Areas.Security.Controllers
{
    [Area("Security")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "امنیت", Icon = "fa-user-secret", Title = "تنظیمات ورود کاربر")]
    [CustomeAuthorizeFilter]
    public class UserLoginConfigController : Controller
    {
        readonly IUserLoginConfigService UserLoginConfigService = null;
        readonly ISiteSettingService SiteSettingService = null;

        public UserLoginConfigController
            (
                IUserLoginConfigService UserLoginConfigService,
                ISiteSettingService SiteSettingService
            )
        {
            this.UserLoginConfigService = UserLoginConfigService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "تنظیمات ورود کاربر", Icon = "fa-sign-in", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "تنظیمات ورود کاربر";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "UserLoginConfig", new { area = "Security" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست تنظیمات ورود کاربر", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Security", "UserLoginConfig")));
        }

        [AreaConfig(Title = "افزودن و ویرایش تنظیمات ورود کاربر جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult CreateUpdate([FromForm] UserLoginConfigCreateUpdateVM input)
        {
            return Json(UserLoginConfigService.CreateUpdate(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده یک تنظیمات ورود کاربر", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult Get()
        {
            return Json(UserLoginConfigService.GetBy(SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
