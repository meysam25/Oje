using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Services;
using Oje.Section.WebMain.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Areas.Controllers
{
    [Area("WebMainAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "وب سایت", Icon = "fa-browser", Title = "درباره ما صفحه اصلی")]
    [CustomeAuthorizeFilter]
    public class AboutUsMainPageController: Controller
    {
        readonly IPropertyService PropertyService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public AboutUsMainPageController
            (
                IPropertyService PropertyService,
                ISiteSettingService SiteSettingService
            )
        {
            this.PropertyService = PropertyService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "درباره ما صفحه اصلی", Icon = "fa-address-card", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "درباره ما صفحه اصلی";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "AboutUsMainPage", new { area = "WebMainAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه درباره ما صفحه اصلی", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("WebMainAdmin", "AboutUsMainPage")));
        }

        [AreaConfig(Title = "افزودن / به روز رسانی درباره ما صفحه اصلی", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult CreateUpdate([FromForm] AboutUsMainPageCreateUpdateVM input)
        {
            return Json(PropertyService.CreateUpdate(input, SiteSettingService.GetSiteSetting()?.Id, PropertyType.AboutUsMainPage));
        }

        [AreaConfig(Title = "مشاهده  درباره ما صفحه اصلی", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult Get()
        {
            return Json(PropertyService.GetBy<AboutUsMainPageVM>(PropertyType.AboutUsMainPage, SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
