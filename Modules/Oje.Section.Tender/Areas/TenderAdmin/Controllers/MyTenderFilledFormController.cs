using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Tender.Interfaces;
using Oje.Section.Tender.Models.View;
using System;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Enums;
using Oje.Section.Tender.Services;

namespace Oje.Section.Tender.Areas.TenderAdmin.Controllers
{
    [Area("TenderAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "سامانه مناقصات", Order = 4, Icon = "fa-funnel-dollar", Title = "مناقصات جدید")]
    [CustomeAuthorizeFilter]
    public class MyTenderFilledFormController : Controller
    {
        readonly ITenderFilledFormService TenderFilledFormService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly ITenderFilledFormPFService TenderFilledFormPFService = null;
        readonly Interfaces.ICompanyService CompanyService = null;
        readonly ITenderFilledFormPriceService TenderFilledFormPriceService = null;
        readonly ITenderFilledFormIssueService TenderFilledFormIssueService = null;
        readonly ITenderProposalFormJsonConfigService TenderProposalFormJsonConfigService = null;
        readonly TenderSelectStatus SelectStatus = TenderSelectStatus.NewTenderUser;

        public MyTenderFilledFormController
            (
                ITenderFilledFormService TenderFilledFormService,
                ISiteSettingService SiteSettingService,
                ITenderFilledFormPFService TenderFilledFormPFService,
                Interfaces.ICompanyService CompanyService,
                ITenderFilledFormPriceService TenderFilledFormPriceService,
                ITenderFilledFormIssueService TenderFilledFormIssueService,
                ITenderProposalFormJsonConfigService TenderProposalFormJsonConfigService
            )
        {
            this.TenderFilledFormService = TenderFilledFormService;
            this.SiteSettingService = SiteSettingService;
            this.TenderFilledFormPFService = TenderFilledFormPFService;
            this.CompanyService = CompanyService;
            this.TenderFilledFormPriceService = TenderFilledFormPriceService;
            this.TenderFilledFormIssueService = TenderFilledFormIssueService;
            this.TenderProposalFormJsonConfigService = TenderProposalFormJsonConfigService;
        }

        [AreaConfig(Title = "مناقصات جدید", Icon = "fa-file-powerpoint", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            //ViewBag.layer = "_WebLayout";
            ViewBag.Title = "مناقصات جدید";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "MyTenderFilledForm", new { area = "TenderAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست مناقصات جدید", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("TenderAdmin", "MyTenderFilledForm")));
        }

        [AreaConfig(Title = "دانلود پی دی اف فرم مناقصه", Icon = "fa-download")]
        [HttpGet]
        public IActionResult DownloadPdf([FromQuery] GlobalLongId input)
        {
            return File(
                    HtmlToPdfBlink.Convert((Request.IsHttps ? "https" : "http") + "://" + Request.Host + Url.Action("PdfDetailes", "MyTenderFilledForm", new { area = "TenderAdmin", id = input.id, isPrint = true }), Request.Cookies),
                    System.Net.Mime.MediaTypeNames.Application.Pdf, DateTime.Now.ToFaDate("_") + "_" + DateTime.Now.ToString("HH_mm_ss") + ".pdf"
                );
        }

        [HttpGet]
        [AreaConfig(Title = "جزییات", Icon = "fa-eye")]
        public IActionResult PdfDetailes([FromQuery] long id, [FromQuery] bool isPrint = false)
        {
            ViewBag.isPrint = isPrint;
            ViewBag.newLayoutName = "_TenderLayout";
            return View(TenderFilledFormService.PdfDetailes(id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser(isPrint)?.UserId, SelectStatus));
        }

        [HttpGet]
        [AreaConfig(Title = "دانلود مدارک", Icon = "fa-download")]
        public IActionResult DownloadDocument([FromQuery] long? id, [FromQuery] int? cId)
        {
            string tempUrl = (Request.IsHttps ? "https" : "http") + "://" + Request.Host + Url.Action("ViewDocument", "MyTenderFilledForm", new { area = "TenderAdmin", tid = id, cId = cId });
            return File(
                    HtmlToPdfBlink.Convert(tempUrl, Request.Cookies),
                    System.Net.Mime.MediaTypeNames.Application.Pdf, DateTime.Now.ToFaDate("_") + "_" + DateTime.Now.ToString("HH_mm_ss") + ".pdf"
                );
        }

        [HttpGet]
        [AreaConfig(Title = "مشاهده مدارک", Icon = "fa-eye")]
        public IActionResult ViewDocument([FromQuery] long? tid, [FromQuery] int? cId)
        {
            //ViewBag.newLayoutName = "_WebLayout";
            ViewBag.isPrint = true;
            ViewBag.documentHtml = TenderFilledFormService.GetDocument(tid, cId, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, SelectStatus);
            return View();
        }

        [AreaConfig(Title = "مشاهده لیست مدارک مناقصات جدید", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetPFormList([FromForm] GlobalGridParentLong searchInput)
        {
            return Json(TenderFilledFormPFService.GetListForWeb(searchInput, searchInput?.pKey, HttpContext.GetLoginUser()?.UserId, SiteSettingService.GetSiteSetting()?.Id, SelectStatus));
        }

        [AreaConfig(Title = "مشاهده کانفیگ تکمیل اطلاعات", Icon = "fa-cog")]
        [HttpPost]
        public ActionResult GetPPFJsonConfig([FromQuery] string id)
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(TenderProposalFormJsonConfigService.GetConsultJsonConfig(id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "ذخیره اطلاعات تکمیلی", Icon = "fa-pen")]
        [HttpPost]
        public IActionResult CreateUpdateValues([FromForm] string pKey)
        {
            return Json(TenderFilledFormService.CreateUpdateConsultationValue(SiteSettingService.GetSiteSetting()?.Id, Request.Form, HttpContext.GetLoginUser()?.UserId, pKey, SelectStatus));
        }

        [AreaConfig(Title = "شماهده اطلاعات تکمیلی", Icon = "fa-pen")]
        [HttpPost]
        public IActionResult GetConsultValues([FromForm] string id)
        {
            return Json(TenderFilledFormService.GetConsultationValue(SiteSettingService.GetSiteSetting()?.Id, Request.Form, HttpContext.GetLoginUser()?.UserId, id, SelectStatus));
        }

        [AreaConfig(Title = "تایید فرم ادمین", Icon = "fa-pen")]
        [HttpPost]
        public IActionResult ConfirmPF([FromForm] string id)
        {
            return Json(TenderFilledFormService.ConfirmPfsForUser(SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, id, SelectStatus));
        }

        [AreaConfig(Title = "مشاهده لیست مناقصات جدید", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] MyTenderFilledFormMainGrid searchInput)
        {
            return Json(TenderFilledFormService.GetListForWeb(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, SelectStatus));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] MyTenderFilledFormMainGrid searchInput)
        {
            var result = TenderFilledFormService.GetListForWeb(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, SelectStatus);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
