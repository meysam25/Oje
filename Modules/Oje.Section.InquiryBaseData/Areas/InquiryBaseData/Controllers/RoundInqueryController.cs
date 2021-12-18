using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.InquiryBaseData.Interfaces;
using Oje.Section.InquiryBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Areas.InquiryBaseData.Controllers
{
    [Area("InquiryBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات استعلام", Icon = "fa-info", Title = "روند کردن جزییات استعلام")]
    [CustomeAuthorizeFilter]
    public class RoundInqueryController: Controller
    {
        readonly IRoundInqueryService RoundInqueryService = null;
        readonly IProposalFormService ProposalFormService = null;
        public RoundInqueryController(IRoundInqueryService RoundInqueryService, IProposalFormService ProposalFormService)
        {
            this.RoundInqueryService = RoundInqueryService;
            this.ProposalFormService = ProposalFormService;
        }

        [AreaConfig(Title = "روند کردن جزییات استعلام", Icon = "fa-balance-scale", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "روند کردن جزییات استعلام";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "RoundInquery", new { area = "InquiryBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست روند کردن جزییات استعلام", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("InquiryBaseData", "RoundInquery")));
        }

        [AreaConfig(Title = "افزودن روند کردن جزییات استعلام جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateRoundInqueryVM input)
        {

            return Json(RoundInqueryService.Create(input));
        }

        [AreaConfig(Title = "حذف روند کردن جزییات استعلام", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(RoundInqueryService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک روند کردن جزییات استعلام", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(RoundInqueryService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  روند کردن جزییات استعلام", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateRoundInqueryVM input)
        {
            return Json(RoundInqueryService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست روند کردن جزییات استعلام", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] RoundInqueryMainGrid searchInput)
        {
            return Json(RoundInqueryService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] RoundInqueryMainGrid searchInput)
        {
            var result = RoundInqueryService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست فرم های پیشنهاد", Icon = "fa-list-alt ")]
        [HttpGet]
        public ActionResult GetProposalFormList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(ProposalFormService.GetSelect2List(searchInput));
        }
    }
}
