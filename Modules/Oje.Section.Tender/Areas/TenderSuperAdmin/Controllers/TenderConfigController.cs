using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Section.Tender.Interfaces;
using Oje.Section.Tender.Models.View;

namespace Oje.Section.Tender.Areas.TenderSuperAdmin.Controllers
{
    [Area("TenderSuperAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "پایه مناقصات", Icon = "fa-funnel-dollar", Title = "تنظیمات مناقصه")]
    [CustomeAuthorizeFilter]
    public class TenderConfigController : Controller
    {
        readonly ITenderConfigService TenderConfigService = null;
        readonly ISiteSettingService SiteSettingService = null;

        public TenderConfigController
            (
                ITenderConfigService TenderConfigService,
                ISiteSettingService SiteSettingService
            )
        {
            this.TenderConfigService = TenderConfigService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "تنظیمات مناقصه", Icon = "fa-cog", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "تنظیمات مناقصه";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "TenderConfig", new { area = "TenderSuperAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه تنظیمات مناقصه", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("TenderSuperAdmin", "TenderConfig")));
        }

        [AreaConfig(Title = "افزودن / به روز رسانی تنظیمات مناقصه", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult CreateUpdate([FromForm] TenderConfigCreateUpdateVM input)
        {
            return Json(TenderConfigService.CreateUpdate(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده تنظیمات مناقصه", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult Get()
        {
            return Json(TenderConfigService.GetBy(SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
