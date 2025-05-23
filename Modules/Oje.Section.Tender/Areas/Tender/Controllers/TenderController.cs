﻿using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Tender.Interfaces;
using Oje.Security.Interfaces;
using Oje.Infrastructure.Exceptions;
using ICompanyService = Oje.Section.Tender.Interfaces.ICompanyService;
using Microsoft.AspNetCore.Mvc;
using System;


namespace Oje.Section.Tender.Areas.Tender.Controllers
{
    [Area("Tender")]
    [Route("[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "مناقصات (وب سایت)", Icon = "fa-funnel-dollar", Title = "ثبت مناقصه")]
    [CustomeAuthorizeFilter]
    public class TenderController : Controller
    {
        readonly ITenderConfigService TenderConfigService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IProposalFormCategoryService ProposalFormCategoryService = null;
        readonly ITenderProposalFormJsonConfigService TenderProposalFormJsonConfigService = null;
        readonly IBlockAutoIpService BlockAutoIpService = null;
        readonly ITenderFilledFormService TenderFilledFormService = null;
        readonly ICompanyService CompanyService = null;
        readonly ITenderProposalFormJsonConfigFileService TenderProposalFormJsonConfigFileService = null;

        public TenderController
            (
                ITenderConfigService TenderConfigService,
                ISiteSettingService SiteSettingService,
                IProposalFormCategoryService ProposalFormCategoryService,
                ITenderProposalFormJsonConfigService TenderProposalFormJsonConfigService,
                IBlockAutoIpService BlockAutoIpService,
                ITenderFilledFormService TenderFilledFormService,
                ICompanyService CompanyService,
                ITenderProposalFormJsonConfigFileService TenderProposalFormJsonConfigFileService
            )
        {
            this.TenderConfigService = TenderConfigService;
            this.SiteSettingService = SiteSettingService;
            this.ProposalFormCategoryService = ProposalFormCategoryService;
            this.TenderProposalFormJsonConfigService = TenderProposalFormJsonConfigService;
            this.BlockAutoIpService = BlockAutoIpService;
            this.TenderFilledFormService = TenderFilledFormService;
            this.CompanyService = CompanyService;
            this.TenderProposalFormJsonConfigFileService = TenderProposalFormJsonConfigFileService;
        }

        [AreaConfig(Title = "ثبت مناقصه", Icon = "fa-file-plus")]
        [HttpGet, HttpPost]
        public IActionResult Index([FromQuery] bool? ignoreMaster)
        {
            ViewBag.Title = "ثبت مناقصه";
            ViewBag.ignoreMaster = ignoreMaster;
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "Tender");

            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه ثبت مناقصه", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Tender", "Tender")));
        }

        [AreaConfig(Title = "تنظیمات صفحه ثبت مناقصه بر اساس بیمه نامه", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetPPFJsonConfig([FromQuery] int? ppfid, [FromQuery] bool? needConsultation)
        {
            Response.ContentType = "application/json; charset=utf-8";

            if (needConsultation == null)
                return Content("{}");

            if (needConsultation == true)
                return Content(TenderProposalFormJsonConfigService.GetJsonConfigBy(ppfid, SiteSettingService.GetSiteSetting()?.Id));

            return Content(TenderProposalFormJsonConfigFileService.GetJsonConfigBy(ppfid, SiteSettingService.GetSiteSetting()?.Id));
        }


        [AreaConfig(Title = "ذخیره", Icon = "fa-pen")]
        [HttpPost]
        public IActionResult Create()
        {
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.CreateTender, BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            var tempResult = TenderFilledFormService.Create(SiteSettingService.GetSiteSetting()?.Id, Request.Form, HttpContext.GetLoginUser()?.UserId);
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.CreateTender, BlockAutoIpAction.AfterExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            return Json(tempResult);
        }

        [HttpGet, HttpPost]
        [AreaConfig(Title = "جزییات", Icon = "fa-pen")]
        public IActionResult PdfDetailes([FromQuery] long id, [FromQuery] bool isPrint = false, [FromQuery] bool? ignoreMaster = null)
        {
            ViewBag.isPrint = isPrint;
            ViewBag.ignoreMaster = ignoreMaster;
            return View(TenderFilledFormService.PdfDetailes(id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, TenderSelectStatus.NewTenderUser));
        }

        [HttpGet]
        [AreaConfig(Title = "دانلود مدارک", Icon = "fa-download")]
        public IActionResult DownloadDocument([FromQuery] long? id, [FromQuery] int? cId)
        {
            string tempUrl = (Request.IsHttps ? "https" : "http") + "://" + Request.Host + Url.Action("ViewDocument", "Tender", new { area = "Tender", tid = id, cId = cId });
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
            ViewBag.documentHtml = TenderFilledFormService.GetDocument(tid, cId, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId);
            return View();
        }

        [AreaConfig(Title = "متن قرارداد", Icon = "fa-pen")]
        [HttpPost]
        public IActionResult GetTermsHtml()
        {
            var foundPPF = TenderConfigService.GetBy(SiteSettingService.GetSiteSetting()?.Id);
            if (foundPPF == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            ViewBag.HtmlTemplate = foundPPF.desctpion;
            ViewBag.companyTitle = SiteSettingService.GetSiteSetting()?.Title;

            return View();
        }

        [AreaConfig(Title = "متن تاییدیه", Icon = "fa-pen")]
        [HttpPost]
        public IActionResult GetConfirmTemplate()
        {
            var foundPPF = TenderConfigService.GetBy(SiteSettingService.GetSiteSetting()?.Id);
            if (foundPPF == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Content(foundPPF.confirmDesc);
        }

        [HttpPost]
        [AreaConfig(Title = "مشاهده لینک مدارک", Icon = "fa-eye")]
        public IActionResult GetPPFCondationFile()
        {
            var foundPPF = TenderConfigService.GetBy(SiteSettingService.GetSiteSetting()?.Id);
            if (foundPPF == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            var ss = SiteSettingService.GetSiteSetting();

            return Content("http" + (ss.IsHttps ? "s" : "") + "://" + ss?.WebsiteUrl + foundPPF.generallow_address);
        }

        [AreaConfig(Title = "لیست گروه بندی بیمه", Icon = "fa-list-alt")]
        [HttpPost]
        public IActionResult GetCategoryList()
        {
            return Json(ProposalFormCategoryService.GetListLight());
        }

        [AreaConfig(Title = "لیست بیمه نامه ها", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetProposalFormList([FromQuery] Select2SearchVM searchInput, [FromQuery] int? insuranceCatId)
        {
            return Json(TenderProposalFormJsonConfigService.GetSelect2List(searchInput, SiteSettingService.GetSiteSetting()?.Id, insuranceCatId));
        }

        [AreaConfig(Title = "لیست شرکت های بیمه", Icon = "fa-list-alt")]
        [HttpPost]
        public IActionResult GetCompanyList()
        {
            return Json(CompanyService.GetLightListString());
        }
    }
}
