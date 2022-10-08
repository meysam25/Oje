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
    [AreaConfig(ModualTitle = "تنظیمات استعلام", Icon = "fa-info", Title = "توضیحات استعلام")]
    [CustomeAuthorizeFilter]
    public class InqueryDescriptionController : Controller
    {
        readonly IInqueryDescriptionService InqueryDescriptionService = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;
        readonly IProposalFormService ProposalFormService = null;
        readonly ICompanyService CompanyService = null;

        public InqueryDescriptionController(
                IInqueryDescriptionService InqueryDescriptionService,
                AccountService.Interfaces.ISiteSettingService SiteSettingService,
                IProposalFormService ProposalFormService,
                ICompanyService CompanyService
            )
        {
            this.InqueryDescriptionService = InqueryDescriptionService;
            this.SiteSettingService = SiteSettingService;
            this.ProposalFormService = ProposalFormService;
            this.CompanyService = CompanyService;
        }

        [AreaConfig(Title = "توضیحات استعلام", Icon = "fa-book", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "توضیحات استعلام";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "InqueryDescription", new { area = "InquiryBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست توضیحات استعلام", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("InquiryBaseData", "InqueryDescription")));
        }

        [AreaConfig(Title = "افزودن توضیحات استعلام جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateInqueryDescriptionVM input)
        {
            return Json(InqueryDescriptionService.Create(input));
        }

        [AreaConfig(Title = "حذف توضیحات استعلام", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(InqueryDescriptionService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک توضیحات استعلام", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(InqueryDescriptionService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  توضیحات استعلام", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateInqueryDescriptionVM input)
        {
            return Json(InqueryDescriptionService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست توضیحات استعلام", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] InqueryDescriptionMainGrid searchInput)
        {
            return Json(InqueryDescriptionService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] InqueryDescriptionMainGrid searchInput)
        {
            var result = InqueryDescriptionService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست وب سایت", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetWebSiteList()
        {
            return Json(SiteSettingService.GetightList());
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
