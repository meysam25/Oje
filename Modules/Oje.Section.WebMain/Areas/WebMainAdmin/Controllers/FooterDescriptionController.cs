using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Filters;
using Oje.Section.WebMain.Models.View;

namespace Oje.Section.WebMain.Areas.WebMainAdmin.Controllers
{
    [Area("WebMainAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات وب سایت", Icon = "fa-browser", Title = "توضیحات فوتر")]
    [CustomeAuthorizeFilter]
    public class FooterDescriptionController : Controller
    {
        readonly IPropertyService PropertyService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public FooterDescriptionController
            (
                IPropertyService PropertyService,
                ISiteSettingService SiteSettingService
            )
        {
            this.PropertyService = PropertyService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "توضیحات فوتر", Icon = "fa-address-card", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "توضیحات فوتر";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "FooterDescription", new { area = "WebMainAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه توضیحات فوتر", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("WebMainAdmin", "FooterDescription")));
        }

        [AreaConfig(Title = "افزودن / به روز رسانی توضیحات فوتر", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult CreateUpdate([FromForm] FooterDescrptionCreateUpdateVM input)
        {
            return Json(PropertyService.CreateUpdate(input, SiteSettingService.GetSiteSetting()?.Id, PropertyType.FooterDescrption));
        }

        [AreaConfig(Title = "مشاهده  توضیحات فوتر", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult Get()
        {
            return Json(PropertyService.GetBy<FooterDescrptionVM>(PropertyType.FooterDescrption, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف", Icon = "fa-trash")]
        [HttpPost]
        public IActionResult Delete([FromQuery] string key)
        {
            return Json(PropertyService.Delete(PropertyType.FooterDescrption, SiteSettingService.GetSiteSetting()?.Id, key));
        }
    }
}
