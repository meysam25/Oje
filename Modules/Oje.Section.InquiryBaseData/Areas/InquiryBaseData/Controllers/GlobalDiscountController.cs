using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.InquiryBaseData.Interfaces;
using Oje.Section.InquiryBaseData.Models.View;
using Microsoft.AspNetCore.Mvc;
using System;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.InquiryBaseData.Areas.InquiryBaseData.Controllers
{
    [Area("InquiryBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات استعلام", Icon = "fa-info", Title = "کمپین (تخفیفات عمومی)")]
    [CustomeAuthorizeFilter]
    public class GlobalDiscountController: Controller
    {
        readonly IGlobalDiscountService GlobalDiscountService = null;
        readonly ICompanyService CompanyService = null;
        readonly IProposalFormService ProposalFormService = null;
        public GlobalDiscountController(
                IGlobalDiscountService GlobalDiscountService,
                ICompanyService CompanyService,
                IProposalFormService ProposalFormService
            )
        {
            this.GlobalDiscountService = GlobalDiscountService;
            this.CompanyService = CompanyService;
            this.ProposalFormService = ProposalFormService;
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
            return Json(GlobalDiscountService.Create(input));
        }

        [AreaConfig(Title = "حذف تخفیفات (کمپین)", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(GlobalDiscountService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک تخفیفات (کمپین)", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(GlobalDiscountService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  تخفیفات (کمپین)", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateGlobalDiscountVM input)
        {
            return Json(GlobalDiscountService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست تخفیفات (کمپین)", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] GlobalDiscountMainGrid searchInput)
        {
            return Json(GlobalDiscountService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] GlobalDiscountMainGrid searchInput)
        {
            var result = GlobalDiscountService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }



        [AreaConfig(Title = "مشاهده لیست شرکت ", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetCompanyList()
        {
            return Json(CompanyService.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست فرم های پیشنهاد", Icon = "fa-list-alt ")]
        [HttpGet]
        public ActionResult GetProposalFormList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(ProposalFormService.GetSelect2List(searchInput));
        }


    }
}
