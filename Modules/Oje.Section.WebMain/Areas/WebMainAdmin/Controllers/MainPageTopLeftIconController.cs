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
    [AreaConfig(ModualTitle = "تنظیمات وب سایت", Icon = "fa-browser", Title = "ایکن های بالا سمت چپ صفحه اصلی")]
    [CustomeAuthorizeFilter]
    public class MainPageTopLeftIconController : Controller
    {
        readonly IPropertyService PropertyService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public MainPageTopLeftIconController(IPropertyService PropertyService, ISiteSettingService SiteSettingService)
        {
            this.PropertyService = PropertyService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "ایکن های بالا سمت چپ صفحه اصلی", Icon = "fa-file-certificate", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "ایکن های بالا سمت چپ صفحه اصلی";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "MainPageTopLeftIcon", new { area = "WebMainAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه ایکن های بالا سمت چپ صفحه اصلی", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("WebMainAdmin", "MainPageTopLeftIcon")));
        }

        [AreaConfig(Title = "افزودن / به روز رسانی ایکن های بالا سمت چپ صفحه اصلی", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult CreateUpdate([FromForm] MainPageTopLeftIconCreateUpdateVM input)
        {
            return Json(PropertyService.CreateUpdate(input, SiteSettingService.GetSiteSetting()?.Id, PropertyType.MainPageTopLeftIcon));
        }

        [AreaConfig(Title = "مشاهده ایکن های بالا سمت چپ صفحه اصلی", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult Get()
        {
            return Json(PropertyService.GetBy<MainPageTopLeftIconVM>(PropertyType.MainPageTopLeftIcon, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف", Icon = "fa-trash")]
        [HttpPost]
        public IActionResult Delete([FromQuery] string key)
        {
            return Json(PropertyService.Delete(PropertyType.MainPageTopLeftIcon, SiteSettingService.GetSiteSetting()?.Id, key));
        }
    }
}
