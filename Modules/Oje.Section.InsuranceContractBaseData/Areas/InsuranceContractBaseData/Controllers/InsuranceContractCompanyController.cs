using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.InsuranceContractBaseData.Interfaces;
using Oje.Section.InsuranceContractBaseData.Models.View;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Oje.Section.InsuranceContractBaseData.Areas.InsuranceContractBaseData.Controllers
{
    [Area("InsuranceContractBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "مدیریت قرارداد ها و مجوز ها", Icon = "fa-file-invoice", Title = "شرکت های بیمه گذار حقوقی")]
    [CustomeAuthorizeFilter]
    public class InsuranceContractCompanyController: Controller
    {
        readonly IInsuranceContractCompanyService InsuranceContractCompanyService = null;
        public InsuranceContractCompanyController(IInsuranceContractCompanyService InsuranceContractCompanyService)
        {
            this.InsuranceContractCompanyService = InsuranceContractCompanyService;
        }

        [AreaConfig(Title = "شرکت های بیمه گذار حقوقی", Icon = "fa-building", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "شرکت های بیمه گذار حقوقی";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "InsuranceContractCompany", new { area = "InsuranceContractBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست شرکت های بیمه گذار حقوقی", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("InsuranceContractBaseData", "InsuranceContractCompany")));
        }

        [AreaConfig(Title = "افزودن شرکت های بیمه گذار حقوقی جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateInsuranceContractCompanyVM input)
        {
            return Json(InsuranceContractCompanyService.Create(input));
        }

        [AreaConfig(Title = "حذف شرکت های بیمه گذار حقوقی", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(InsuranceContractCompanyService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک شرکت های بیمه گذار حقوقی", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(InsuranceContractCompanyService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  شرکت های بیمه گذار حقوقی", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateInsuranceContractCompanyVM input)
        {
            return Json(InsuranceContractCompanyService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست شرکت های بیمه گذار حقوقی", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] InsuranceContractCompanyMainGrid searchInput)
        {
            return Json(InsuranceContractCompanyService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] InsuranceContractCompanyMainGrid searchInput)
        {
            var result = InsuranceContractCompanyService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
