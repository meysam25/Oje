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
    [AreaConfig(ModualTitle = "تنظیمات استعلام", Icon = "fa-info", Title = "حداکثر تخفیف")]
    [CustomeAuthorizeFilter]
    public class InquiryMaxDiscountController : Controller
    {
        readonly IInquiryMaxDiscountManager InquiryMaxDiscountManager = null;
        readonly ICompanyManager CompanyManager = null;
        readonly IProposalFormManager ProposalFormManager = null;
        public InquiryMaxDiscountController(
                IInquiryMaxDiscountManager InquiryMaxDiscountManager,
                ICompanyManager CompanyManager,
                IProposalFormManager ProposalFormManager
            )
        {
            this.InquiryMaxDiscountManager = InquiryMaxDiscountManager;
            this.CompanyManager = CompanyManager;
            this.ProposalFormManager = ProposalFormManager;
        }

        [AreaConfig(Title = "حداکثر تخفیف", Icon = "fa-tags", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "حداکثر تخفیف";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "InquiryMaxDiscount", new { area = "InquiryBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست حداکثر تخفیف", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("InquiryBaseData", "InquiryMaxDiscount")));
        }

        [AreaConfig(Title = "افزودن حداکثر تخفیف جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateInquiryMaxDiscountVM input)
        {
            return Json(InquiryMaxDiscountManager.Create(input));
        }

        [AreaConfig(Title = "حذف حداکثر تخفیف", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(InquiryMaxDiscountManager.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک حداکثر تخفیف", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(InquiryMaxDiscountManager.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  حداکثر تخفیف", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateInquiryMaxDiscountVM input)
        {
            return Json(InquiryMaxDiscountManager.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست حداکثر تخفیف", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] InquiryMaxDiscountMainGrid searchInput)
        {
            return Json(InquiryMaxDiscountManager.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] InquiryMaxDiscountMainGrid searchInput)
        {
            var result = InquiryMaxDiscountManager.GetList(searchInput);
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
