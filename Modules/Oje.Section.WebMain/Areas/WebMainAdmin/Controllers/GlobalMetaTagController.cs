using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;

namespace Oje.Section.WebMain.Areas.WebMainAdmin.Controllers
{
    [Area("WebMainAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات وب سایت", Icon = "fa-browser", Title = "متا تگ در کلیه صفحه ها")]
    [CustomeAuthorizeFilter]
    public class GlobalMetaTagController : Controller
    {
        readonly IPropertyService PropertyService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public GlobalMetaTagController
            (
                IPropertyService PropertyService,
                ISiteSettingService SiteSettingService
            )
        {
            this.PropertyService = PropertyService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "متا تگ در کلیه صفحه ها", Icon = "fa-file-certificate", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "متا تگ در کلیه صفحه ها";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "GlobalMetaTag", new { area = "WebMainAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه متا تگ در کلیه صفحه ها", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("WebMainAdmin", "GlobalMetaTag")));
        }

        [AreaConfig(Title = "افزودن / به روز رسانی متا تگ در کلیه صفحه ها", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult CreateUpdate([FromForm] GlobalMetaTagCreateUpdateVM input)
        {
            return Json(PropertyService.CreateUpdate(input, SiteSettingService.GetSiteSetting()?.Id, PropertyType.GlobalMetaTag));
        }

        [AreaConfig(Title = "مشاهده متا تگ در کلیه صفحه ها", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult Get()
        {
            return Json(PropertyService.GetBy<GlobalMetaTagCreateUpdateVM>(PropertyType.GlobalMetaTag, SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
