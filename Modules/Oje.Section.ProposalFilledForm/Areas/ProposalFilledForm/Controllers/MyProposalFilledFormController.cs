using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.View;
using Oje.Security.Interfaces;
using System;
using System.Collections.Generic;

namespace Oje.Section.ProposalFilledForm.Areas.ProposalFilledForm.Controllers
{
    [Area("ProposalFilledForm")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "فرم های پیشنهاد", Icon = "fa-file-powerpoint", Title = "بیمه نامه های من")]
    [CustomeAuthorizeFilter]
    public class MyProposalFilledFormController : Controller
    {
        readonly List<ProposalFilledFormStatus> validStatus =
            new List<ProposalFilledFormStatus>() { ProposalFilledFormStatus.New, ProposalFilledFormStatus.W8ForConfirm, ProposalFilledFormStatus.NeedSpecialist };

        readonly ISiteSettingService SiteSettingService = null;
        readonly IProposalFilledFormAdminService ProposalFilledFormAdminService = null;
        readonly IBlockAutoIpService BlockAutoIpService = null;
        readonly IProposalFilledFormDocumentService ProposalFilledFormDocumentService = null;
        readonly IProposalFilledFormStatusLogService ProposalFilledFormStatusLogService = null;
        readonly IProposalFormCategoryService ProposalFormCategoryService = null;
        readonly ProposalFormService.Interfaces.IProposalFormService ProposalFormService = null;

        public MyProposalFilledFormController
            (
                ISiteSettingService SiteSettingService,
                IProposalFilledFormAdminService ProposalFilledFormAdminService,
                IBlockAutoIpService BlockAutoIpService,
                IProposalFilledFormDocumentService ProposalFilledFormDocumentService,
                IProposalFilledFormStatusLogService ProposalFilledFormStatusLogService,
                IProposalFormCategoryService ProposalFormCategoryService,
                ProposalFormService.Interfaces.IProposalFormService ProposalFormService
            )
        {
            this.SiteSettingService = SiteSettingService;
            this.ProposalFilledFormAdminService = ProposalFilledFormAdminService;
            this.BlockAutoIpService = BlockAutoIpService;
            this.ProposalFilledFormDocumentService = ProposalFilledFormDocumentService;
            this.ProposalFilledFormStatusLogService = ProposalFilledFormStatusLogService;
            this.ProposalFormCategoryService = ProposalFormCategoryService;
            this.ProposalFormService = ProposalFormService;
        }

        [AreaConfig(Title = "لیست بیمه نامه های من", Icon = "fa-book", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "لیست بیمه نامه های من";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "MyProposalFilledForm", new { area = "ProposalFilledForm" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست بیمه نامه های من", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("ProposalFilledForm", "MyProposalFilledForm")));
        }

        [AreaConfig(Title = "دانلود پی دی اف بیمه نامه های من", Icon = "fa-download")]
        [HttpGet]
        public IActionResult DownloadPdf([FromQuery] GlobalLongId input)
        {
            return File(
                    HtmlToPdfBlink.Convert((Request.IsHttps ? "https" : "http") + "://" + Request.Host + Url.Action("Detaile", "MyProposalFilledForm", new { area = "ProposalFilledForm", id = input.id, isPrint = true }), Request.Cookies),
                    System.Net.Mime.MediaTypeNames.Application.Pdf, DateTime.Now.ToFaDate("_") + "_" + DateTime.Now.ToString("HH_mm_ss") + ".pdf"
                );
        }

        [AreaConfig(Title = "جزییات بیمه نامه های من", Icon = "fa-eye")]
        [HttpGet]
        public IActionResult Detaile([FromQuery] long id, [FromQuery] bool isPrint = false)
        {
            ViewBag.isPrint = isPrint;
            ViewBag.newLayoutName = "_WebLayout";
            return View("PdfDetailesForAdmin", ProposalFilledFormAdminService.PdfDetailes(id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, null, validStatus));
        }

        [AreaConfig(Title = "مشاهده لیست بیمه نامه های من", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetList([FromForm] MyProposalFilledFormMainGrid searchInput)
        {
            return Json(ProposalFilledFormAdminService.GetListForUser(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, validStatus));
        }

        [AreaConfig(Title = "مشاهده اسناد بیمه نامه های من", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetPPFImageList([FromForm] GlobalGridParentLong input)
        {
            return Json(ProposalFilledFormAdminService.GetUploadImages(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, null, validStatus));
        }

        [AreaConfig(Title = "افزودن اسناد بیمه نامه های من", Icon = "fa-plus")]
        [HttpPost]
        public ActionResult UploadNewFile([FromForm] long? pKey, [FromForm] IFormFile mainFile)
        {
            var tempResult = ProposalFilledFormAdminService.UploadImage(pKey, mainFile, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, null, validStatus);
            return Json(tempResult);
        }

        [AreaConfig(Title = "مشاهده لیست مدارک مالی بیمه نامه های من", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetDocumentList([FromForm] ProposalFilledFormDocumentMainGrid searchInput)
        {
            return Json(ProposalFilledFormDocumentService.GetList(new ProposalFilledFormDocumentMainGrid() { skip = searchInput.skip, take = searchInput.take }, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, null, validStatus));
        }

        [AreaConfig(Title = "مشاهده لیست تاریخچه وضعیت فرم پیشنهاد تایید شده", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetStatusLogList([FromForm] ProposalFilledFormLogMainGrid searchInput)
        {
            return Json(ProposalFilledFormStatusLogService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, validStatus));
        }

        [AreaConfig(Title = "مشاهده لیست گروه بندی فرم پیشنهاد", Icon = "fa-eye")]
        [HttpGet]
        public ActionResult GetProposalFormCategoryList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(ProposalFormCategoryService.GetSelect2List(searchInput));
        }

        [AreaConfig(Title = "مشاهده لیست فرم های پیشنهاد", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetProposalFormList([FromQuery] Select2SearchVM searchInput, [FromQuery] int? ppfCatId)
        {
            return Json(ProposalFormService.GetSelect2List(searchInput, ppfCatId));
        }
    }
}
