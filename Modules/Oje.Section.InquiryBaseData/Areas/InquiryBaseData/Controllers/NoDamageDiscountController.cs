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

namespace Oje.Section.BaseData.Areas.BaseData.Controllers
{
    [Area("InquiryBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات استعلام",  Order = 10,Icon = "fa-info", Title = "تخفیف عدم خسارت")]
    [CustomeAuthorizeFilter]
    public class NoDamageDiscountController: Controller
    {
        readonly INoDamageDiscountService NoDamageDiscountService = null;
        readonly ICompanyService CompanyService = null;
        readonly IProposalFormService ProposalFormService = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;

        public NoDamageDiscountController(
                INoDamageDiscountService NoDamageDiscountService, 
                ICompanyService CompanyService, 
                IProposalFormService ProposalFormService,
                AccountService.Interfaces.ISiteSettingService SiteSettingService
            )
        {
            this.NoDamageDiscountService = NoDamageDiscountService;
            this.CompanyService = CompanyService;
            this.ProposalFormService = ProposalFormService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "تخفیف عدم خسارت", Icon = "fa-percent", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "تخفیف عدم خسارت";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "NoDamageDiscount", new { area = "InquiryBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست تخفیف عدم خسارت", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("InquiryBaseData", "NoDamageDiscount")));
        }

        [AreaConfig(Title = "افزودن تخفیف عدم خسارت جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateNoDamageDiscountVM input)
        {
            return Json(NoDamageDiscountService.Create(input));
        }

        [AreaConfig(Title = "حذف تخفیف عدم خسارت", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(NoDamageDiscountService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک تخفیف عدم خسارت", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(NoDamageDiscountService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  تخفیف عدم خسارت", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateNoDamageDiscountVM input)
        {
            return Json(NoDamageDiscountService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست تخفیف عدم خسارت", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] NoDamageDiscountMainGrid searchInput)
        {
            return Json(NoDamageDiscountService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] NoDamageDiscountMainGrid searchInput)
        {
            var result = NoDamageDiscountService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست شرکت های بیمه", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCompanyList()
        {
            return Json(CompanyService.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست شرکت های بیمه", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetPPFList(Select2SearchVM searchInput, [FromQuery] int? cSOWSiteSettingId)
        {
            return Json(ProposalFormService.GetSelect2List(searchInput, HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
