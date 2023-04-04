using Microsoft.AspNetCore.Mvc;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Enums;
using Oje.AccountService.Filters;
using Oje.Section.InsuranceContractBaseData.Models.View;

namespace Oje.Section.InsuranceContractBaseData.Areas.InsuranceContractBaseDataSuperAdmin.Controllers
{
    [Area("InsuranceContractBaseDataSuperAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "مدیریت قرارداد ها و مجوز ها", Icon = "fa-file-invoice", Title = "توضیحات صفحه اصلی")]
    [CustomeAuthorizeFilter]
    public class IndexPageDescriptionController: Controller
    {
        readonly IPropertyService PropertyService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public IndexPageDescriptionController
            (
                IPropertyService PropertyService,
                ISiteSettingService SiteSettingService
            )
        {
            this.PropertyService = PropertyService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "توضیحات صفحه اصلی", Icon = "fa-address-card", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "توضیحات صفحه اصلی";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "IndexPageDescription", new { area = "InsuranceContractBaseDataSuperAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه توضیحات صفحه اصلی", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("InsuranceContractBaseDataSuperAdmin", "IndexPageDescription")));
        }

        [AreaConfig(Title = "افزودن / به روز رسانی توضیحات صفحه اصلی", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult CreateUpdate([FromForm] IndexPageDescriptionCreateUpdateVM input)
        {
            return Json(PropertyService.CreateUpdate(input, SiteSettingService.GetSiteSetting()?.Id, PropertyType.TreatmentIndexPageDescription));
        }

        [AreaConfig(Title = "مشاهده  توضیحات صفحه اصلی", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult Get()
        {
            return Json(PropertyService.GetBy<IndexPageDescriptionVM>(PropertyType.TreatmentIndexPageDescription, SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
