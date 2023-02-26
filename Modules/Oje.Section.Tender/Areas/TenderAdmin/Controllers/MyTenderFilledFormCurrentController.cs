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
    [AreaConfig(ModualTitle = "مناقصات", Icon = "fa-funnel-dollar", Title = "مناقصات جاری")]
    [CustomeAuthorizeFilter]
    public class MyTenderFilledFormCurrentController : Controller
    {
        readonly ITenderFilledFormService TenderFilledFormService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly ITenderFilledFormPFService TenderFilledFormPFService = null;
        readonly Interfaces.ICompanyService CompanyService = null;
        readonly ITenderFilledFormPriceService TenderFilledFormPriceService = null;
        readonly ITenderFilledFormIssueService TenderFilledFormIssueService = null;
        readonly TenderSelectStatus SelectStatus = TenderSelectStatus.CurrentTenderUser;

        public MyTenderFilledFormCurrentController
            (
                ITenderFilledFormService TenderFilledFormService,
                ISiteSettingService SiteSettingService,
                ITenderFilledFormPFService TenderFilledFormPFService,
                Interfaces.ICompanyService CompanyService,
                ITenderFilledFormPriceService TenderFilledFormPriceService,
                ITenderFilledFormIssueService TenderFilledFormIssueService
            )
        {
            this.TenderFilledFormService = TenderFilledFormService;
            this.SiteSettingService = SiteSettingService;
            this.TenderFilledFormPFService = TenderFilledFormPFService;
            this.CompanyService = CompanyService;
            this.TenderFilledFormPriceService = TenderFilledFormPriceService;
            this.TenderFilledFormIssueService = TenderFilledFormIssueService;
        }

        [AreaConfig(Title = "مناقصات جاری", Icon = "fa-file-powerpoint", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            //ViewBag.layer = "_WebLayout";
            ViewBag.Title = "مناقصات جاری";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "MyTenderFilledFormCurrent", new { area = "TenderAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست مناقصات جاری", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("TenderAdmin", "MyTenderFilledFormCurrent")));
        }

        [AreaConfig(Title = "مشاهده تاریخ یک مناقصه", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetDatesById([FromForm] GlobalLongId input)
        {
            return Json(TenderFilledFormService.GetDatesByForWeb(input?.id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, SelectStatus));
        }

        [AreaConfig(Title = "مشاهده دسترسی نماینده یک مناقصه", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetAccessById([FromForm] GlobalLongId input)
        {
            return Json(TenderFilledFormService.GetAgentAccessByForWeb(input?.id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, SelectStatus));
        }

        [AreaConfig(Title = "به روز رسانی تاریخ مناقصه", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult UpdateDates([FromForm] MyTenderFilledFormUpdateDateVM input)
        {
            return Json(TenderFilledFormService.UpdateDatesForWeb(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, SelectStatus));
        }

        [AreaConfig(Title = "به روز رسانی دسترسی نمایده به مناقصه", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult UpdateAccess([FromForm] MyTenderFilledFormUpdateAgentAccessVM input)
        {
            return Json(TenderFilledFormService.UpdateAgentAccessForWeb(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, SelectStatus));
        }

        [AreaConfig(Title = "انتشار مناقصه", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Publish([FromForm] GlobalLongId input)
        {
            return Json(TenderFilledFormService.UpdatePublishForWeb(input?.id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, SelectStatus));
        }

        [AreaConfig(Title = "دانلود پی دی اف فرم مناقصه", Icon = "fa-download")]
        [HttpGet]
        public IActionResult DownloadPdf([FromQuery] GlobalLongId input)
        {
            return File(
                    HtmlToPdfBlink.Convert((Request.IsHttps ? "https" : "http") + "://" + Request.Host + Url.Action("PdfDetailes", "MyTenderFilledFormCurrent", new { area = "TenderAdmin", id = input.id, isPrint = true }), Request.Cookies),
                    System.Net.Mime.MediaTypeNames.Application.Pdf, DateTime.Now.ToFaDate("_") + "_" + DateTime.Now.ToString("HH_mm_ss") + ".pdf"
                );
        }

        [HttpGet]
        [AreaConfig(Title = "جزییات", Icon = "fa-eye")]
        public IActionResult PdfDetailes([FromQuery] long id, [FromQuery] bool isPrint = false)
        {
            ViewBag.isPrint = isPrint;
            ViewBag.newLayoutName = "_TenderLayout";
            return View(TenderFilledFormService.PdfDetailes(id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, SelectStatus));
        }

        [HttpGet]
        [AreaConfig(Title = "دانلود مدارک", Icon = "fa-download")]
        public IActionResult DownloadDocument([FromQuery] long? id, [FromQuery] int? cId)
        {
            string tempUrl = (Request.IsHttps ? "https" : "http") + "://" + Request.Host + Url.Action("ViewDocument", "MyTenderFilledFormCurrent", new { area = "TenderAdmin", tid = id, cId = cId });
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

        [AreaConfig(Title = "مشاهده لیست مدارک مناقصات جاری", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetPFormList([FromForm] GlobalGridParentLong searchInput)
        {
            return Json(TenderFilledFormPFService.GetListForWeb(searchInput, searchInput?.pKey, HttpContext.GetLoginUser()?.UserId, SiteSettingService.GetSiteSetting()?.Id, SelectStatus));
        }

        [AreaConfig(Title = "مشاهده لیست مناقصات جاری", Icon = "fa-list-alt")]
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

        [AreaConfig(Title = "مشاهده لیست شرکت", Icon = "fa-list-alt")]
        [HttpPost]
        public IActionResult GetCompanyList()
        {
            return Json(CompanyService.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست بیمه ها", Icon = "fa-list-alt")]
        [HttpPost]
        public IActionResult GetInsuranceList([FromForm] GlobalGridParentLong input)
        {
            return Json(TenderFilledFormService.GetInsuranceList(input?.pKey, SiteSettingService.GetSiteSetting()?.Id, SelectStatus));
        }
    }
}
