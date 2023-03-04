using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Tender.Interfaces;
using Oje.Section.Tender.Models.View;
using System;
using Oje.AccountService.Interfaces;

namespace Oje.Section.Tender.Areas.TenderAdmin.Controllers
{
    [Area("TenderAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "سامانه مناقصات", Order = 4, Icon = "fa-funnel-dollar", Title = "مناقصات برنده شده")]
    [CustomeAuthorizeFilter]
    public class TenderFilledFormIssueTenderController : Controller
    {
        readonly ITenderFilledFormService TenderFilledFormService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly ITenderFilledFormPFService TenderFilledFormPFService = null;
        readonly Interfaces.ICompanyService CompanyService = null;
        readonly ITenderFilledFormIssueService TenderFilledFormIssueService = null;
        readonly TenderSelectStatus tenderSelectStatus = TenderSelectStatus.IssuedTender;

        public TenderFilledFormIssueTenderController
           (
               ITenderFilledFormService TenderFilledFormService,
               ISiteSettingService SiteSettingService,
               ITenderFilledFormPFService TenderFilledFormPFService,
               Interfaces.ICompanyService CompanyService,
               ITenderFilledFormIssueService TenderFilledFormIssueService
           )
        {
            this.TenderFilledFormService = TenderFilledFormService;
            this.SiteSettingService = SiteSettingService;
            this.TenderFilledFormPFService = TenderFilledFormPFService;
            this.CompanyService = CompanyService;
            this.TenderFilledFormIssueService = TenderFilledFormIssueService;
        }

        [AreaConfig(Title = "مناقصات برنده شده", Icon = "fa-file-powerpoint", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "مناقصات برنده شده";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "TenderFilledFormIssueTender", new { area = "TenderAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست مناقصات برنده شده", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("TenderAdmin", "TenderFilledFormIssueTender")));
        }

        [AreaConfig(Title = "دانلود پی دی اف فرم مناقصات برنده شده", Icon = "fa-download")]
        [HttpGet]
        public IActionResult DownloadPdf([FromQuery] GlobalLongId input)
        {
            return File(
                    HtmlToPdfBlink.Convert((Request.IsHttps ? "https" : "http") + "://" + Request.Host + Url.Action("PdfDetailes", "TenderFilledFormIssueTender", new { area = "TenderAdmin", id = input.id, isPrint = true }), Request.Cookies),
                    System.Net.Mime.MediaTypeNames.Application.Pdf, DateTime.Now.ToFaDate("_") + "_" + DateTime.Now.ToString("HH_mm_ss") + ".pdf"
                );
        }

        [HttpGet]
        [AreaConfig(Title = "جزییات", Icon = "fa-eye")]
        public IActionResult PdfDetailes([FromQuery] long id, [FromQuery] bool isPrint = false)
        {
            ViewBag.isPrint = isPrint;
            //ViewBag.newLayoutName = "_WebLayout";
            return View(TenderFilledFormService.PdfDetailes(id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser(isPrint)?.UserId, tenderSelectStatus));
        }

        [HttpGet]
        [AreaConfig(Title = "دانلود مدارک", Icon = "fa-download")]
        public IActionResult DownloadDocument([FromQuery] long? id, [FromQuery] int? cId)
        {
            string tempUrl = (Request.IsHttps ? "https" : "http") + "://" + Request.Host + Url.Action("ViewDocument", "TenderFilledFormIssueTender", new { area = "TenderAdmin", tid = id, cId = cId });
            return File(
                    HtmlToPdfBlink.Convert(tempUrl, Request.Cookies),
                    System.Net.Mime.MediaTypeNames.Application.Pdf, DateTime.Now.ToFaDate("_") + "_" + DateTime.Now.ToString("HH_mm_ss") + ".pdf"
                );
        }

        [HttpGet]
        [AreaConfig(Title = "مشاهده مدارک", Icon = "fa-eye")]
        public IActionResult ViewDocument([FromQuery] long? tid, [FromQuery] int? cId)
        {
            ViewBag.isPrint = true;
            ViewBag.documentHtml = TenderFilledFormService.GetDocument(tid, cId, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, tenderSelectStatus);
            return View();
        }

        [AreaConfig(Title = "مشاهده لیست مدارک مناقصات برنده شده", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetPFormList([FromForm] GlobalGridParentLong searchInput)
        {
            return Json(TenderFilledFormPFService.GetListForWeb(searchInput, searchInput?.pKey, HttpContext.GetLoginUser()?.UserId, SiteSettingService.GetSiteSetting()?.Id, tenderSelectStatus));
        }

        [AreaConfig(Title = "مشاهده لیست مناقصات برنده شده", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] TenderFilledFormMainGrid searchInput)
        {
            return Json(TenderFilledFormService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, tenderSelectStatus));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] TenderFilledFormMainGrid searchInput)
        {
            var result = TenderFilledFormService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, tenderSelectStatus);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "افزودن صدور جدید برای مناقصات برنده شده", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult IssuePPF([FromForm] TenderFilledFormIssueCreateUpdateVM input)
        {
            return Json(TenderFilledFormIssueService.Create(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, tenderSelectStatus));
        }

        [AreaConfig(Title = "مشاهده یک مناقصات برنده شده صادر شده", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetIssueById([FromForm] GlobalLongId input)
        {
            return Json(TenderFilledFormIssueService.GetBy(input?.id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, tenderSelectStatus));
        }

        [AreaConfig(Title = "مشاهده لیست بیمه نامه های صادره", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetIssueList([FromForm] GlobalGridParentLong searchInput)
        {
            return Json(TenderFilledFormIssueService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, tenderSelectStatus));
        }


        [AreaConfig(Title = "مشاهده لیست شرکت", Icon = "fa-list-alt")]
        [HttpPost]
        public IActionResult GetCompanyList()
        {
            return Json(CompanyService.GetLightList(HttpContext.GetLoginUser()?.UserId));
        }

        [AreaConfig(Title = "مشاهده لیست بیمه ها", Icon = "fa-list-alt")]
        [HttpPost]
        public IActionResult GetInsuranceList([FromForm] GlobalGridParentLong input)
        {
            return Json(TenderFilledFormService.GetInsuranceList(input?.pKey, SiteSettingService.GetSiteSetting()?.Id, tenderSelectStatus));
        }
    }
}