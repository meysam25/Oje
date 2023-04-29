using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.InsuranceContractBaseData.Interfaces;
using Oje.Section.InsuranceContractBaseData.Models.View;
using Oje.AccountService.Interfaces;
using System;


namespace Oje.Section.InsuranceContractBaseData.Areas.InsuranceContractBaseDataSuperAdmin.Controllers
{
    [Area("InsuranceContractBaseDataSuperAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "مدیریت قرارداد ها و مجوز ها", Icon = "fa-file-invoice", Title = "بنر صفحه اصلی")]
    [CustomeAuthorizeFilter]
    public class TreatmentMainSliderController: Controller
    {
        readonly ISiteSettingService SiteSettingService = null;
        readonly ITreatmentMainSliderService TreatmentMainSliderService = null;

        public TreatmentMainSliderController(
            ISiteSettingService SiteSettingService,
            ITreatmentMainSliderService TreatmentMainSliderService
            )
        {
            this.SiteSettingService = SiteSettingService;
            this.TreatmentMainSliderService = TreatmentMainSliderService;
        }

        [AreaConfig(Title = "بنر صفحه اصلی", Icon = "fa-images", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "بنر صفحه اصلی";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "TreatmentMainSlider", new { area = "InsuranceContractBaseDataSuperAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست بنر صفحه اصلی", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("InsuranceContractBaseDataSuperAdmin", "TreatmentMainSlider")));
        }

        [AreaConfig(Title = "افزودن بنر صفحه اصلی جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] TreatmentMainSliderCreateUpdateVM input)
        {
            return Json(TreatmentMainSliderService.Create(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId));
        }

        [AreaConfig(Title = "حذف بنر صفحه اصلی", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(TreatmentMainSliderService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده یک بنر صفحه اصلی", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(TreatmentMainSliderService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  بنر صفحه اصلی", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] TreatmentMainSliderCreateUpdateVM input)
        {
            return Json(TreatmentMainSliderService.Update(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId));
        }

        [AreaConfig(Title = "مشاهده لیست بنر صفحه اصلی", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] TreatmentMainSliderMainGrid searchInput)
        {
            return Json(TreatmentMainSliderService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] TreatmentMainSliderMainGrid searchInput)
        {
            var result = TreatmentMainSliderService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
