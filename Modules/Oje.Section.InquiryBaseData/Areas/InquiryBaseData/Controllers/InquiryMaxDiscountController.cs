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
    [AreaConfig(ModualTitle = "تنظیمات استعلام",  Order = 10,Icon = "fa-info", Title = "حداکثر تخفیف")]
    [CustomeAuthorizeFilter]
    public class InquiryMaxDiscountController : Controller
    {
        readonly IInquiryMaxDiscountService InquiryMaxDiscountService = null;
        readonly ICompanyService CompanyService = null;
        readonly IProposalFormService ProposalFormService = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;

        public InquiryMaxDiscountController(
                IInquiryMaxDiscountService InquiryMaxDiscountService,
                ICompanyService CompanyService,
                IProposalFormService ProposalFormService,
                AccountService.Interfaces.ISiteSettingService SiteSettingService
            )
        {
            this.InquiryMaxDiscountService = InquiryMaxDiscountService;
            this.CompanyService = CompanyService;
            this.ProposalFormService = ProposalFormService;
            this.SiteSettingService = SiteSettingService;
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
            return Json(InquiryMaxDiscountService.Create(input));
        }

        [AreaConfig(Title = "حذف حداکثر تخفیف", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(InquiryMaxDiscountService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک حداکثر تخفیف", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(InquiryMaxDiscountService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  حداکثر تخفیف", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateInquiryMaxDiscountVM input)
        {
            return Json(InquiryMaxDiscountService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست حداکثر تخفیف", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] InquiryMaxDiscountMainGrid searchInput)
        {
            return Json(InquiryMaxDiscountService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] InquiryMaxDiscountMainGrid searchInput)
        {
            var result = InquiryMaxDiscountService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست شرکت ", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCompanyList()
        {
            return Json(CompanyService.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست فرم های پیشنهاد", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetProposalFormList([FromQuery] Select2SearchVM searchInput, [FromQuery]int? cSOWSiteSettingId)
        {
            return Json(ProposalFormService.GetSelect2List(searchInput, HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
