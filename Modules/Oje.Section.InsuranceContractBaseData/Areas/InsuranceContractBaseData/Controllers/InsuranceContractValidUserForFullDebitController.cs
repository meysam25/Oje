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
    [AreaConfig(ModualTitle = "مدیریت قرارداد ها و مجوز ها", Icon = "fa-file-invoice", Title = "فروش از دم قسط")]
    [CustomeAuthorizeFilter]
    public class InsuranceContractValidUserForFullDebitController: Controller
    {
        readonly IInsuranceContractValidUserForFullDebitService InsuranceContractValidUserForFullDebitService = null;
        readonly IInsuranceContractService InsuranceContractService = null;
        public InsuranceContractValidUserForFullDebitController
            (
                IInsuranceContractValidUserForFullDebitService InsuranceContractValidUserForFullDebitService,
                IInsuranceContractService InsuranceContractService
            )
        {
            this.InsuranceContractValidUserForFullDebitService = InsuranceContractValidUserForFullDebitService;
            this.InsuranceContractService = InsuranceContractService;
        }

        [AreaConfig(Title = "فروش از دم قسط", Icon = "fa-credit-card", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "فروش از دم قسط";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "InsuranceContractValidUserForFullDebit", new { area = "InsuranceContractBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست فروش از دم قسط", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("InsuranceContractBaseData", "InsuranceContractValidUserForFullDebit")));
        }

        [AreaConfig(Title = "افزودن فروش از دم قسط جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateInsuranceContractValidUserForFullDebitVM input)
        {
            return Json(InsuranceContractValidUserForFullDebitService.Create(input));
        }

        [AreaConfig(Title = "افزودن فروش از دم قسط از روی فایل اکسل", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult CreateFromXcel([FromForm] GlobalExcelFile input)
        {
            return Json(InsuranceContractValidUserForFullDebitService.CreateFromExcel(input));
        }

        [AreaConfig(Title = "حذف فروش از دم قسط", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(InsuranceContractValidUserForFullDebitService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک فروش از دم قسط", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalLongId input)
        {
            return Json(InsuranceContractValidUserForFullDebitService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  فروش از دم قسط", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateInsuranceContractValidUserForFullDebitVM input)
        {
            return Json(InsuranceContractValidUserForFullDebitService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست فروش از دم قسط", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] InsuranceContractValidUserForFullDebitMainGrid searchInput)
        {
            return Json(InsuranceContractValidUserForFullDebitService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] InsuranceContractValidUserForFullDebitMainGrid searchInput)
        {
            var result = InsuranceContractValidUserForFullDebitService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();


            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده قرارداد", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetContractList()
        {
            return Json(InsuranceContractService.GetLightList());
        }
    }
}
