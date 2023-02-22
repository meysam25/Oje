using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Filters;
using Oje.Section.Tender.Models.View;

namespace Oje.Section.WebMain.Areas.Controllers
{
    [Area("TenderAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "مناقصات", Icon = "fa-funnel-dollar", Title = "افتخارات ما")]
    [CustomeAuthorizeFilter]
    public class OurPrideController: Controller
    {
        readonly IPropertyService PropertyService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public OurPrideController
            (
                IPropertyService PropertyService,
                ISiteSettingService SiteSettingService
            )
        {
            this.PropertyService = PropertyService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "افتخارات ما صفحه اصلی", Icon = "fa-award", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "افتخارات ما صفحه اصلی";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "OurPride", new { area = "TenderAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه افتخارات ما صفحه اصلی", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("TenderAdmin", "OurPride")));
        }

        [AreaConfig(Title = "افزودن / به روز رسانی افتخارات ما صفحه اصلی", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult CreateUpdate([FromForm] OurPrideCreateUpdateVM input)
        {
            return Json(PropertyService.CreateUpdate(input, SiteSettingService.GetSiteSetting()?.Id, PropertyType.OurPrideMainPageForTender));
        }

        [AreaConfig(Title = "مشاهده افتخارات ما صفحه اصلی", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult Get()
        {
            return Json(PropertyService.GetBy<OurPrideVM>(PropertyType.OurPrideMainPageForTender, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف", Icon = "fa-trash")]
        [HttpPost]
        public IActionResult Delete([FromQuery] string key)
        {
            return Json(PropertyService.Delete(PropertyType.OurPrideMainPageForTender, SiteSettingService.GetSiteSetting()?.Id, key));
        }
    }
}
