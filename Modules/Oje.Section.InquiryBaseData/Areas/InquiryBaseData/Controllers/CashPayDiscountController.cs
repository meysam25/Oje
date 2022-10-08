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
    [AreaConfig(ModualTitle = "تنظیمات استعلام", Icon = "fa-info", Title = "تخفیف نقدی خرید")]
    [CustomeAuthorizeFilter]
    public class CashPayDiscountController : Controller
    {
        readonly ICashPayDiscountService CashPayDiscountService = null;
        readonly ICompanyService CompanyService = null;
        readonly IProposalFormService ProposalFormService = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;

        public CashPayDiscountController(
                ICashPayDiscountService CashPayDiscountService,
                ICompanyService CompanyService,
                IProposalFormService ProposalFormService,
                AccountService.Interfaces.ISiteSettingService SiteSettingService
            )
        {
            this.CashPayDiscountService = CashPayDiscountService;
            this.CompanyService = CompanyService;
            this.ProposalFormService = ProposalFormService;
            this.SiteSettingService = SiteSettingService;
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
            return Json(CashPayDiscountService.Create(input));
        }

        [AreaConfig(Title = "حذف تخفیف نقدی خرید", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(CashPayDiscountService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک تخفیف نقدی خرید", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(CashPayDiscountService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  تخفیف نقدی خرید", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateCashPayDiscountVM input)
        {
            return Json(CashPayDiscountService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست تخفیف نقدی خرید", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] CashPayDiscountMainGrid searchInput)
        {
            return Json(CashPayDiscountService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] CashPayDiscountMainGrid searchInput)
        {
            var result = CashPayDiscountService.GetList(searchInput);
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
        public ActionResult GetProposalFormList([FromQuery] Select2SearchVM searchInput, [FromQuery] int? cSOWSiteSettingId)
        {
            return Json(ProposalFormService.GetSelect2List(searchInput, HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
