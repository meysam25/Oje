using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.InsuranceContractBaseData.Interfaces;
using Oje.Section.InsuranceContractBaseData.Models.View;
using Microsoft.AspNetCore.Mvc;
using System;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.InsuranceContractBaseData.Areas.InsuranceContractBaseData.Controllers
{
    [Area("InsuranceContractBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "مدیریت قرارداد ها و مجوز ها", Icon = "fa-file-invoice", Title = "نوع قراردادها")]
    [CustomeAuthorizeFilter]
    public class InsuranceContractTypeController: Controller
    {
        readonly IInsuranceContractTypeService InsuranceContractTypeService = null;
        public InsuranceContractTypeController(IInsuranceContractTypeService InsuranceContractTypeService)
        {
            this.InsuranceContractTypeService = InsuranceContractTypeService;
        }

        [AreaConfig(Title = "نوع قراردادها", Icon = "fa-object-group", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "نوع قراردادها";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "InsuranceContractType", new { area = "InsuranceContractBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست نوع قراردادها", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("InsuranceContractBaseData", "InsuranceContractType")));
        }

        [AreaConfig(Title = "افزودن نوع قراردادها جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateInsuranceContractTypeVM input)
        {
            return Json(InsuranceContractTypeService.Create(input));
        }

        [AreaConfig(Title = "حذف نوع قراردادها", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(InsuranceContractTypeService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده یک نوع قراردادها", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(InsuranceContractTypeService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  نوع قراردادها", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateInsuranceContractTypeVM input)
        {
            return Json(InsuranceContractTypeService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست نوع قراردادها", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] InsuranceContractTypeMainGrid searchInput)
        {
            return Json(InsuranceContractTypeService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] InsuranceContractTypeMainGrid searchInput)
        {
            var result = InsuranceContractTypeService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
