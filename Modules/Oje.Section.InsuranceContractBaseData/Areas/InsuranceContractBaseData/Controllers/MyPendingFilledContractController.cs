using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.InsuranceContractBaseData.Interfaces;
using Oje.Section.InsuranceContractBaseData.Models.View;
using System;
using System.Collections.Generic;

namespace Oje.Section.InsuranceContractBaseData.Areas.InsuranceContractBaseData.Controllers
{
    [Area("InsuranceContractBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "سامانه درمان وب", Order = 5, Icon = "fa-file-invoice", Title = "خسارت های در دست برسی")]
    [CustomeAuthorizeFilter]
    public class MyPendingFilledContractController: Controller
    {
        readonly IMyFilledContractService MyFilledContractService = null;
        readonly IInsuranceContractProposalFilledFormService InsuranceContractProposalFilledFormService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IInsuranceContractProposalFilledFormStatusLogService InsuranceContractProposalFilledFormStatusLogService = null;

        readonly List<InsuranceContractProposalFilledFormType> validStatus = new List<InsuranceContractProposalFilledFormType>()
            {
                InsuranceContractProposalFilledFormType.Pending
            };

        public MyPendingFilledContractController
                (
                    IMyFilledContractService MyFilledContractService,
                    ISiteSettingService SiteSettingService,
                    IInsuranceContractProposalFilledFormService InsuranceContractProposalFilledFormService,
                    IInsuranceContractProposalFilledFormStatusLogService InsuranceContractProposalFilledFormStatusLogService
                )
        {
            this.MyFilledContractService = MyFilledContractService;
            this.SiteSettingService = SiteSettingService;
            this.InsuranceContractProposalFilledFormService = InsuranceContractProposalFilledFormService;
            this.InsuranceContractProposalFilledFormStatusLogService = InsuranceContractProposalFilledFormStatusLogService;
        }

        [AreaConfig(Title = "خسارت های در دست برسی", Icon = "fa-file-search", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "خسارت های در دست برسی";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "MyPendingFilledContract", new { area = "InsuranceContractBaseData" });
            ViewBag.layer = SiteSettingService.GetSiteSetting()?.Layout;

            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست خسارت های در دست برسی", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("InsuranceContractBaseData", "MyPendingFilledContract")));
        }

        [AreaConfig(Title = "مشاهده جززیات خسارت ثبت شده", Icon = "fa-eye")]
        [HttpGet]
        public IActionResult Detaile([FromQuery] long? id, [FromQuery] bool isPrint = false)
        {
            ViewBag.isPrint = isPrint;
            ViewBag.newLayoutName = SiteSettingService.GetSiteSetting()?.Layout;

            return View("~/Views/Contract/Detaile.cshtml", InsuranceContractProposalFilledFormService.Detaile(id, HttpContext.GetLoginUser(isPrint)?.UserId, SiteSettingService.GetSiteSetting()?.Id, false, validStatus));
        }

        [AreaConfig(Title = "دانلود پی دی اف فرم پیشنهاد", Icon = "fa-download")]
        [HttpGet]
        public IActionResult DownloadPdf([FromQuery] GlobalLongId input)
        {
            return File(
                    HtmlToPdfBlink.Convert((Request.IsHttps ? "https" : "http") + "://" + Request.Host + Url.Action("Detaile", "MyPendingFilledContract", new { area = "InsuranceContractBaseData", id = input.id, isPrint = true }), Request.Cookies),
                    System.Net.Mime.MediaTypeNames.Application.Pdf, DateTime.Now.ToFaDate("_") + "_" + DateTime.Now.ToString("HH_mm_ss") + ".pdf"
                );
        }

        [AreaConfig(Title = "مشاهده لیست خسارت های در دست برسی", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] MyFilledContractMainGrid searchInput)
        {
            return Json(MyFilledContractService.GetList(searchInput, HttpContext.GetLoginUser()?.UserId, SiteSettingService.GetSiteSetting()?.Id, validStatus));
        }

        [AreaConfig(Title = "مشاهده مدارک لیست خسارت های ثبت شده", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetPPFImageList([FromForm] GlobalGridParentLong input)
        {
            return Json(MyFilledContractService.GetPPFImageList(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, validStatus));
        }

        [AreaConfig(Title = "مشاهده لیست تاریخچه", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetStatusLogList([FromForm] InsuranceContractProposalFilledFormStatusLogGrid searchInput)
        {
            return Json(InsuranceContractProposalFilledFormStatusLogService.GetListForUser(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, validStatus));
        }
    }
}
