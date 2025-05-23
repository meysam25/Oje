﻿using Microsoft.AspNetCore.Http;
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
    [AreaConfig(ModualTitle = "مدیریت بیمه نامه ها", Order = 7,  Icon = "fa-file-powerpoint", Title = "بیمه نامه های من (لغو شده)")]
    [CustomeAuthorizeFilter]
    public class MyNotIssueProposalFilledFormController: Controller
    {
        readonly List<ProposalFilledFormStatus> validStatus =
            new List<ProposalFilledFormStatus>() { ProposalFilledFormStatus.NotIssue };

        readonly ISiteSettingService SiteSettingService = null;
        readonly IProposalFilledFormAdminService ProposalFilledFormAdminService = null;
        readonly IBlockAutoIpService BlockAutoIpService = null;
        readonly IProposalFilledFormDocumentService ProposalFilledFormDocumentService = null;
        readonly IProposalFilledFormStatusLogService ProposalFilledFormStatusLogService = null;
        readonly IProposalFormCategoryService ProposalFormCategoryService = null;
        readonly ProposalFormService.Interfaces.IProposalFormService ProposalFormService = null;

        public MyNotIssueProposalFilledFormController
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

        [AreaConfig(Title = "بیمه نامه های من (لغو شده)", Icon = "fa-book-dead", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "بیمه نامه های من (لغو شده)";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "MyNotIssueProposalFilledForm", new { area = "ProposalFilledForm" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه بیمه نامه های من (لغو شده)", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("ProposalFilledForm", "MyNotIssueProposalFilledForm")));
        }

        [AreaConfig(Title = "دانلود پی دی اف بیمه نامه های من (لغو شده)", Icon = "fa-download")]
        [HttpGet]
        public IActionResult DownloadPdf([FromQuery] GlobalLongId input)
        {
            return File(
                    HtmlToPdfBlink.Convert((Request.IsHttps ? "https" : "http") + "://" + Request.Host + Url.Action("Detaile", "MyNotIssueProposalFilledForm", new { area = "ProposalFilledForm", id = input.id, isPrint = true }), Request.Cookies),
                    System.Net.Mime.MediaTypeNames.Application.Pdf, DateTime.Now.ToFaDate("_") + "_" + DateTime.Now.ToString("HH_mm_ss") + ".pdf"
                );
        }

        [AreaConfig(Title = "جزییات بیمه نامه های من (لغو شده)", Icon = "fa-eye")]
        [HttpGet]
        public IActionResult Detaile([FromQuery] long id, [FromQuery] bool isPrint = false)
        {
            ViewBag.isPrint = isPrint;
            ViewBag.newLayoutName = "_WebLayout";
            return View("PdfDetailes", ProposalFilledFormAdminService.PdfDetailes(id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, null, validStatus));
        }

        [AreaConfig(Title = "مشاهده بیمه نامه های من (لغو شده)", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetList([FromForm] MyProposalFilledFormMainGrid searchInput)
        {
            return Json(ProposalFilledFormAdminService.GetListForUser(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, validStatus));
        }

        [AreaConfig(Title = "مشاهده اسناد بیمه نامه های من (لغو شده)", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetPPFImageList([FromForm] GlobalGridParentLong input)
        {
            return Json(ProposalFilledFormAdminService.GetUploadImages(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, null, validStatus));
        }

        [AreaConfig(Title = "افزودن اسناد بیمه نامه های من (لغو شده)", Icon = "fa-plus")]
        [HttpPost]
        public ActionResult UploadNewFile([FromForm] long? pKey, [FromForm] IFormFile mainFile)
        {
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.NewDocumentForWebUserPPF, BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            var tempResult = ProposalFilledFormAdminService.UploadImage(pKey, mainFile, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, null, validStatus);
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.NewDocumentForWebUserPPF, BlockAutoIpAction.AfterExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            return Json(tempResult);
        }

        [AreaConfig(Title = "مشاهده مدارک مالی بیمه نامه های من (لغو شده)", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetDocumentList([FromForm] ProposalFilledFormDocumentMainGrid searchInput)
        {
            return Json(ProposalFilledFormDocumentService.GetList(new ProposalFilledFormDocumentMainGrid() { skip = searchInput.skip, take = searchInput.take }, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, null, validStatus));
        }

        [AreaConfig(Title = "مشاهده تاریخچه وضعیت فرم پیشنهاد تایید شده", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetStatusLogList([FromForm] ProposalFilledFormLogMainGrid searchInput)
        {
            return Json(ProposalFilledFormStatusLogService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, validStatus));
        }

        [AreaConfig(Title = "مشاهده گروه بندی فرم پیشنهاد", Icon = "fa-eye")]
        [HttpGet]
        public ActionResult GetProposalFormCategoryList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(ProposalFormCategoryService.GetSelect2List(searchInput));
        }

        [AreaConfig(Title = "مشاهده فرم های پیشنهاد", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetProposalFormList([FromQuery] Select2SearchVM searchInput, [FromQuery] int? ppfCatId, [FromQuery] int? cSOWSiteSettingId)
        {
            return Json(ProposalFormService.GetSelect2List(searchInput, ppfCatId, HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
