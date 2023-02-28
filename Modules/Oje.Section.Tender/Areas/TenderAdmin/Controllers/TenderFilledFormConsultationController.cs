using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Tender.Interfaces;
using Oje.Section.Tender.Models.View;
using Oje.Security.Services;
using System;

namespace Oje.Section.Tender.Areas.TenderAdmin.Controllers
{
    [Area("TenderAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "مناقصات", Icon = "fa-funnel-dollar", Title = "مشاوره مناقصه")]
    [CustomeAuthorizeFilter]
    public class TenderFilledFormConsultationController : Controller
    {
        readonly ITenderFilledFormService TenderFilledFormService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly ITenderFilledFormPFService TenderFilledFormPFService = null;
        readonly Interfaces.ICompanyService CompanyService = null;
        readonly ITenderFilledFormIssueService TenderFilledFormIssueService = null;
        readonly ITenderProposalFormJsonConfigService TenderProposalFormJsonConfigService = null;
        readonly TenderSelectStatus SelectStatus = TenderSelectStatus.Consultation;

        public TenderFilledFormConsultationController
           (
              ITenderFilledFormService TenderFilledFormService,
              ISiteSettingService SiteSettingService,
              ITenderFilledFormPFService TenderFilledFormPFService,
              Interfaces.ICompanyService CompanyService,
              ITenderFilledFormIssueService TenderFilledFormIssueService,
              ITenderProposalFormJsonConfigService TenderProposalFormJsonConfigService
           )
        {
            this.TenderFilledFormService = TenderFilledFormService;
            this.SiteSettingService = SiteSettingService;
            this.TenderFilledFormPFService = TenderFilledFormPFService;
            this.CompanyService = CompanyService;
            this.TenderFilledFormIssueService = TenderFilledFormIssueService;
            this.TenderProposalFormJsonConfigService = TenderProposalFormJsonConfigService;
        }

        [AreaConfig(Title = "مشاوره مناقصه", Icon = "fa-file-powerpoint", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "مشاوره مناقصه";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "TenderFilledFormConsultation", new { area = "TenderAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست مشاوره مناقصه", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("TenderAdmin", "TenderFilledFormConsultation")));
        }

        [AreaConfig(Title = "دانلود پی دی اف فرم مشاوره مناقصه", Icon = "fa-download")]
        [HttpGet]
        public IActionResult DownloadPdf([FromQuery] GlobalLongId input)
        {
            return File(
                    HtmlToPdfBlink.Convert((Request.IsHttps ? "https" : "http") + "://" + Request.Host + Url.Action("PdfDetailes", "TenderFilledFormConsultation", new { area = "TenderAdmin", id = input.id, isPrint = true }), Request.Cookies),
                    System.Net.Mime.MediaTypeNames.Application.Pdf, DateTime.Now.ToFaDate("_") + "_" + DateTime.Now.ToString("HH_mm_ss") + ".pdf"
                );
        }

        [HttpGet]
        [AreaConfig(Title = "جزییات", Icon = "fa-eye")]
        public IActionResult PdfDetailes([FromQuery] long id, [FromQuery] bool isPrint = false)
        {
            ViewBag.isPrint = isPrint;
            return View(TenderFilledFormService.PdfDetailes(id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, SelectStatus));
        }

        [HttpGet]
        [AreaConfig(Title = "دانلود مدارک", Icon = "fa-download")]
        public IActionResult DownloadDocument([FromQuery] long? id, [FromQuery] int? cId)
        {
            string tempUrl = (Request.IsHttps ? "https" : "http") + "://" + Request.Host + Url.Action("ViewDocument", "TenderFilledFormConsultation", new { area = "TenderAdmin", tid = id, cId = cId });
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
            ViewBag.documentHtml = TenderFilledFormService.GetDocument(tid, cId, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, SelectStatus);
            return View();
        }

        [AreaConfig(Title = "مشاهده لیست مدارک مشاوره مناقصه", Icon = "fa-list-alt")]
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
            return Json(TenderFilledFormService.ConfirmPfs(SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, id, SelectStatus));
        }

        [AreaConfig(Title = "مشاهده لیست مشاوره مناقصه", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] TenderFilledFormMainGrid searchInput)
        {
            return Json(TenderFilledFormService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, SelectStatus));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] TenderFilledFormMainGrid searchInput)
        {
            var result = TenderFilledFormService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, SelectStatus);
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
            return Json(CompanyService.GetLightList(HttpContext.GetLoginUser()?.UserId));
        }

        [AreaConfig(Title = "مشاهده لیست بیمه ها", Icon = "fa-list-alt")]
        [HttpPost]
        public IActionResult GetInsuranceList([FromForm] GlobalGridParentLong input)
        {
            return Json(TenderFilledFormService.GetInsuranceList(input?.pKey, SiteSettingService.GetSiteSetting()?.Id, SelectStatus));
        }

        [AreaConfig(Title = "مشاهده مدارک مناقصه", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetFileList([FromForm] GlobalGridParentLong input)
        {
            return Json(TenderFilledFormService.GetUploadFiles(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, SelectStatus));
        }

        [AreaConfig(Title = "حذف مدارک مناقصه", Icon = "fa-trash-o")]
        [HttpPost]
        public ActionResult DeleteFile([FromForm] GlobalLongId input, [FromForm] long? pKey)
        {
            return Json(TenderFilledFormService.DeleteUploadFile(input?.id, pKey, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, SelectStatus));
        }

        [AreaConfig(Title = "افزودن مدارک مناقصه", Icon = "fa-plus")]
        [HttpPost]
        public ActionResult UploadNewFile([FromForm] long? pKey, [FromForm] IFormFile mainFile, [FromForm] string title)
        {
            return Json(TenderFilledFormService.UploadNewFile(pKey, mainFile, title, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, SelectStatus));
        }
    }
}
