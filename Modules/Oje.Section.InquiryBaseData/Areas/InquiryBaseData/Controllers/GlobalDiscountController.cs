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
    [AreaConfig(ModualTitle = "تنظیمات استعلام", Icon = "fa-info", Title = "کمپین (تخفیفات عمومی)")]
    [CustomeAuthorizeFilter]
    public class GlobalDiscountController: Controller
    {
        readonly IGlobalDiscountManager GlobalDiscountManager = null;
        readonly ICompanyManager CompanyManager = null;
        readonly IProposalFormManager ProposalFormManager = null;
        public GlobalDiscountController(
                IGlobalDiscountManager GlobalDiscountManager,
                ICompanyManager CompanyManager,
                IProposalFormManager ProposalFormManager
            )
        {
            this.GlobalDiscountManager = GlobalDiscountManager;
            this.CompanyManager = CompanyManager;
            this.ProposalFormManager = ProposalFormManager;
        }

        [AreaConfig(Title = "تخفیفات (کمپین)", Icon = "fa-percent", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "تخفیفات (کمپین)";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "GlobalDiscount", new { area = "InquiryBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست تخفیفات (کمپین)", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("InquiryBaseData", "GlobalDiscount")));
        }

        [AreaConfig(Title = "افزودن تخفیفات (کمپین) جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateGlobalDiscountVM input)
        {
            return Json(GlobalDiscountManager.Create(input));
        }

        [AreaConfig(Title = "حذف تخفیفات (کمپین)", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(GlobalDiscountManager.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک تخفیفات (کمپین)", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(GlobalDiscountManager.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  تخفیفات (کمپین)", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateGlobalDiscountVM input)
        {
            return Json(GlobalDiscountManager.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست تخفیفات (کمپین)", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] GlobalDiscountMainGrid searchInput)
        {
            return Json(GlobalDiscountManager.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] GlobalDiscountMainGrid searchInput)
        {
            var result = GlobalDiscountManager.GetList(searchInput);
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
