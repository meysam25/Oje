using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.InquiryBaseData.Interfaces;
using Oje.Section.InquiryBaseData.Models.View;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Areas.InquiryBaseData.Controllers
{
    [Area("InquiryBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات استعلام", Icon = "fa-info", Title = "تخفیفات تفاهم نامه")]
    [CustomeAuthorizeFilter]
    public class InsuranceContractDiscountController: Controller
    {
        readonly IInsuranceContractDiscountService InsuranceContractDiscountService = null;
        readonly ICompanyService CompanyService = null;
        readonly IInsuranceContractService InsuranceContractService = null;
        public InsuranceContractDiscountController(
                IInsuranceContractDiscountService InsuranceContractDiscountService,
                ICompanyService CompanyService,
                IInsuranceContractService InsuranceContractService
            )
        {
            this.InsuranceContractDiscountService = InsuranceContractDiscountService;
            this.CompanyService = CompanyService;
            this.InsuranceContractService = InsuranceContractService;
        }

        [AreaConfig(Title = "تخفیفات تفاهم نامه", Icon = "fa-balance-scale", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "تخفیفات تفاهم نامه";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "InsuranceContractDiscount", new { area = "InquiryBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست تخفیفات تفاهم نامه", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("InquiryBaseData", "InsuranceContractDiscount")));
        }

        [AreaConfig(Title = "افزودن تخفیفات تفاهم نامه جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateInsuranceContractDiscountVM input)
        {
            return Json(InsuranceContractDiscountService.Create(input));
        }

        [AreaConfig(Title = "حذف تخفیفات تفاهم نامه", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(InsuranceContractDiscountService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک تخفیفات تفاهم نامه", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(InsuranceContractDiscountService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  تخفیفات تفاهم نامه", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateInsuranceContractDiscountVM input)
        {
            return Json(InsuranceContractDiscountService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست تخفیفات تفاهم نامه", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] InsuranceContractDiscountMainGrid searchInput)
        {
            return Json(InsuranceContractDiscountService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] InsuranceContractDiscountMainGrid searchInput)
        {
            var result = InsuranceContractDiscountService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست شرکت ", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetCompanyList()
        {
            return Json(CompanyService.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست قرارداد ", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetContractList()
        {
            return Json(InsuranceContractService.GetLightList());
        }
    }
}
