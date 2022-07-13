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
    [AreaConfig(ModualTitle = "وب سایت", Icon = "fa-browser", Title = "شبکه اجتماعی فوتر")]
    [CustomeAuthorizeFilter]
    public class FooterSocialLinksController: Controller
    {
        readonly IPropertyService PropertyService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public FooterSocialLinksController
            (
                IPropertyService PropertyService,
                ISiteSettingService SiteSettingService
            )
        {
            this.PropertyService = PropertyService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "شبکه اجتماعی فوتر", Icon = "fa-share-alt", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "شبکه اجتماعی فوتر";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "FooterSocialLinks", new { area = "WebMainAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه شبکه اجتماعی فوتر", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("WebMainAdmin", "FooterSocialLinks")));
        }

        [AreaConfig(Title = "افزودن / به روز رسانی شبکه اجتماعی فوتر", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult CreateUpdate([FromForm] FooterSocialIconCreateUpdateVM input)
        {
            return Json(PropertyService.CreateUpdate(input, SiteSettingService.GetSiteSetting()?.Id, PropertyType.FooterIcon));
        }

        [AreaConfig(Title = "مشاهده  شبکه اجتماعی فوتر", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult Get()
        {
            return Json(PropertyService.GetBy<FooterSocialIconCreateUpdateVM>(PropertyType.FooterIcon, SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
