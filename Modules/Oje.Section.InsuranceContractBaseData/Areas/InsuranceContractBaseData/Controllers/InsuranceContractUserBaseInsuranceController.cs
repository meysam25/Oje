using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.InsuranceContractBaseData.Interfaces;
using Oje.Section.InsuranceContractBaseData.Models.View;
using System;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.InsuranceContractBaseData.Areas.InsuranceContractBaseData.Controllers
{
    [Area("InsuranceContractBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "مدیریت قرارداد ها و مجوز ها", Icon = "fa-file-invoice", Title = "لیست بیمه پایه بیمه شدگان")]
    [CustomeAuthorizeFilter]
    public class InsuranceContractUserBaseInsuranceController: Controller
    {
        readonly ISiteSettingService SiteSettingService = null;
        readonly IInsuranceContractUserBaseInsuranceService InsuranceContractUserBaseInsuranceService = null;
        public InsuranceContractUserBaseInsuranceController
            (
                ISiteSettingService SiteSettingService,
                IInsuranceContractUserBaseInsuranceService InsuranceContractUserBaseInsuranceService
            )
        {
            this.SiteSettingService = SiteSettingService;
            this.InsuranceContractUserBaseInsuranceService = InsuranceContractUserBaseInsuranceService;
        }

        [AreaConfig(Title = "لیست بیمه پایه بیمه شدگان", Icon = "fa-object-group", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "لیست بیمه پایه بیمه شدگان";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "InsuranceContractUserBaseInsurance", new { area = "InsuranceContractBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست بیمه پایه بیمه شدگان", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("InsuranceContractBaseData", "InsuranceContractUserBaseInsurance")));
        }

        [AreaConfig(Title = "افزودن لیست بیمه پایه بیمه شدگان جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] InsuranceContractUserBaseInsuranceCreateUpdateVM input)
        {
            return Json(InsuranceContractUserBaseInsuranceService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف بیمه پایه بیمه شدگان", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(InsuranceContractUserBaseInsuranceService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده یک  بیمه پایه بیمه شدگان", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(InsuranceContractUserBaseInsuranceService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی بیمه پایه بیمه شدگان", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] InsuranceContractUserBaseInsuranceCreateUpdateVM input)
        {
            return Json(InsuranceContractUserBaseInsuranceService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست بیمه پایه بیمه شدگان", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] InsuranceContractUserBaseInsuranceMainGrid searchInput)
        {
            return Json(InsuranceContractUserBaseInsuranceService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] InsuranceContractUserBaseInsuranceMainGrid searchInput)
        {
            var result = InsuranceContractUserBaseInsuranceService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
