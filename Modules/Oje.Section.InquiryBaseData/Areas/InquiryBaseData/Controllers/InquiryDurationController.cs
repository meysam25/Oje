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
    [AreaConfig(ModualTitle = "تنظیمات استعلام", Icon = "fa-info", Title = "مدت زمان بیمه نامه")]
    [CustomeAuthorizeFilter]
    public class InquiryDurationController: Controller
    {
        readonly IInquiryDurationManager InquiryDurationManager = null;
        readonly ICompanyManager CompanyManager = null;
        readonly IProposalFormManager ProposalFormManager = null;
        public InquiryDurationController(
                IInquiryDurationManager InquiryDurationManager,
                ICompanyManager CompanyManager,
                IProposalFormManager ProposalFormManager
            )
        {
            this.InquiryDurationManager = InquiryDurationManager;
            this.CompanyManager = CompanyManager;
            this.ProposalFormManager = ProposalFormManager;
        }

        [AreaConfig(Title = "مدت زمان بیمه نامه", Icon = "fa-calendar-day", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "مدت زمان بیمه نامه";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "InquiryDuration", new { area = "InquiryBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست مدت زمان بیمه نامه", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("InquiryBaseData", "InquiryDuration")));
        }

        [AreaConfig(Title = "افزودن مدت زمان بیمه نامه جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateInquiryDurationVM input)
        {
            return Json(InquiryDurationManager.Create(input));
        }

        [AreaConfig(Title = "حذف مدت زمان بیمه نامه", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(InquiryDurationManager.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک مدت زمان بیمه نامه", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(InquiryDurationManager.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  مدت زمان بیمه نامه", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateInquiryDurationVM input)
        {
            return Json(InquiryDurationManager.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست مدت زمان بیمه نامه", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] InquiryDurationMainGrid searchInput)
        {
            return Json(InquiryDurationManager.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] InquiryDurationMainGrid searchInput)
        {
            var result = InquiryDurationManager.GetList(searchInput);
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
