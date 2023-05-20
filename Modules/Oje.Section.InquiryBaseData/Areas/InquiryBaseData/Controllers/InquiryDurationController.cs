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
    [AreaConfig(ModualTitle = "تنظیمات استعلام",  Order = 10,Icon = "fa-info", Title = "مدت زمان بیمه نامه")]
    [CustomeAuthorizeFilter]
    public class InquiryDurationController: Controller
    {
        readonly IInquiryDurationService InquiryDurationService = null;
        readonly ICompanyService CompanyService = null;
        readonly IProposalFormService ProposalFormService = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;

        public InquiryDurationController(
                IInquiryDurationService InquiryDurationService,
                ICompanyService CompanyService,
                IProposalFormService ProposalFormService,
                AccountService.Interfaces.ISiteSettingService SiteSettingService
            )
        {
            this.InquiryDurationService = InquiryDurationService;
            this.CompanyService = CompanyService;
            this.ProposalFormService = ProposalFormService;
            this.SiteSettingService = SiteSettingService;
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
            return Json(InquiryDurationService.Create(input));
        }

        [AreaConfig(Title = "حذف مدت زمان بیمه نامه", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(InquiryDurationService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک مدت زمان بیمه نامه", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(InquiryDurationService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  مدت زمان بیمه نامه", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateInquiryDurationVM input)
        {
            return Json(InquiryDurationService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست مدت زمان بیمه نامه", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] InquiryDurationMainGrid searchInput)
        {
            return Json(InquiryDurationService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] InquiryDurationMainGrid searchInput)
        {
            var result = InquiryDurationService.GetList(searchInput);
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
        public ActionResult GetProposalFormList([FromQuery] Select2SearchVM searchInput, [FromQuery] int? cSOWSiteSettingId)
        {
            return Json(ProposalFormService.GetSelect2List(searchInput, HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
