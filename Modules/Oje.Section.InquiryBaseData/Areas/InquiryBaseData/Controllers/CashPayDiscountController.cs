using Oje.AccountManager.Filters;
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
    [AreaConfig(ModualTitle = "تنظیمات استعلام", Icon = "fa-info", Title = "تخفیف نقدی خرید")]
    [CustomeAuthorizeFilter]
    public class CashPayDiscountController : Controller
    {
        readonly ICashPayDiscountManager CashPayDiscountManager = null;
        readonly ICompanyManager CompanyManager = null;
        readonly IProposalFormManager ProposalFormManager = null;
        public CashPayDiscountController(
                ICashPayDiscountManager CashPayDiscountManager,
                ICompanyManager CompanyManager,
                IProposalFormManager ProposalFormManager
            )
        {
            this.CashPayDiscountManager = CashPayDiscountManager;
            this.CompanyManager = CompanyManager;
            this.ProposalFormManager = ProposalFormManager;
        }

        [AreaConfig(Title = "تخفیف نقدی خرید", Icon = "fa-percent", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "تخفیف نقدی خرید";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "CashPayDiscount", new { area = "InquiryBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست تخفیف نقدی خرید", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("InquiryBaseData", "CashPayDiscount")));
        }

        [AreaConfig(Title = "افزودن تخفیف نقدی خرید جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateCashPayDiscountVM input)
        {
            return Json(CashPayDiscountManager.Create(input));
        }

        [AreaConfig(Title = "حذف تخفیف نقدی خرید", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(CashPayDiscountManager.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک تخفیف نقدی خرید", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(CashPayDiscountManager.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  تخفیف نقدی خرید", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateCashPayDiscountVM input)
        {
            return Json(CashPayDiscountManager.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست تخفیف نقدی خرید", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] CashPayDiscountMainGrid searchInput)
        {
            return Json(CashPayDiscountManager.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] CashPayDiscountMainGrid searchInput)
        {
            var result = CashPayDiscountManager.GetList(searchInput);
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
            return Json(CompanyManager.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست فرم های پیشنهاد", Icon = "fa-list-alt ")]
        [HttpGet]
        public ActionResult GetProposalFormList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(ProposalFormManager.GetSelect2List(searchInput));
        }
    }
}
