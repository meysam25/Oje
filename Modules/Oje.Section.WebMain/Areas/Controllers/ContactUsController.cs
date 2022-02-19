using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Filters;
using Oje.Section.WebMain.Models.View;

namespace Oje.Section.WebMain.Areas.Controllers
{
    [Area("WebMainAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "وب سایت", Icon = "fa-browser", Title = "تماس با ما")]
    [CustomeAuthorizeFilter]
    public class ContactUsController: Controller
    {
        readonly ISiteSettingService SiteSettingService = null;
        readonly IPropertyService PropertyService = null;
        public ContactUsController
            (
                ISiteSettingService SiteSettingService,
                IPropertyService PropertyService
            )
        {
            this.SiteSettingService = SiteSettingService;
            this.PropertyService = PropertyService;
        }

        [AreaConfig(Title = "تماس با ما", Icon = "fa-address-card", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "تماس با ما";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ContactUs", new { area = "WebMainAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه تماس با ما", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("WebMainAdmin", "ContactUs")));
        }

        [AreaConfig(Title = "افزودن / به روز رسانی تماس با ما", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult CreateUpdate([FromForm] ContactUsCreateUpdateVM input)
        {
            return Json(PropertyService.CreateUpdate(input, SiteSettingService.GetSiteSetting()?.Id, PropertyType.ContactUs));
        }

        [AreaConfig(Title = "مشاهده  درباره ما صفحه اصلی", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult Get()
        {
            return Json(PropertyService.GetBy<ContactUsVM>(PropertyType.ContactUs, SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
