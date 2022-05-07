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
using Oje.Security.Interfaces;
using System;
using System.Collections.Generic;

namespace Oje.Section.InsuranceContractBaseData.Areas.InsuranceContractBaseData.Controllers
{
    [Area("InsuranceContractBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "مدیریت قرارداد ها و مجوز ها", Icon = "fa-file-invoice", Title = "درانتظار دریافت اصل مدرک")]
    [CustomeAuthorizeFilter]
    public class MyW8ForReciveingRealDocumentsFilledContractController: Controller
    {
        readonly IMyFilledContractService MyFilledContractService = null;
        readonly IInsuranceContractProposalFilledFormService InsuranceContractProposalFilledFormService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IInsuranceContractProposalFilledFormStatusLogService InsuranceContractProposalFilledFormStatusLogService = null;
        readonly IBlockAutoIpService BlockAutoIpService = null;

        readonly List<InsuranceContractProposalFilledFormType> validStatus = new List<InsuranceContractProposalFilledFormType>()
            {
                InsuranceContractProposalFilledFormType.W8ForReciveingRealDocuments
            };

        public MyW8ForReciveingRealDocumentsFilledContractController
                (
                    IMyFilledContractService MyFilledContractService,
                    ISiteSettingService SiteSettingService,
                    IInsuranceContractProposalFilledFormService InsuranceContractProposalFilledFormService,
                    IInsuranceContractProposalFilledFormStatusLogService InsuranceContractProposalFilledFormStatusLogService,
                    IBlockAutoIpService BlockAutoIpService
                )
        {
            this.MyFilledContractService = MyFilledContractService;
            this.SiteSettingService = SiteSettingService;
            this.InsuranceContractProposalFilledFormService = InsuranceContractProposalFilledFormService;
            this.InsuranceContractProposalFilledFormStatusLogService = InsuranceContractProposalFilledFormStatusLogService;
            this.BlockAutoIpService = BlockAutoIpService;
        }

        [AreaConfig(Title = "درانتظار دریافت اصل مدرک", Icon = "fa-file-signature", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "درانتظار دریافت اصل مدرک";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "MyW8ForReciveingRealDocumentsFilledContract", new { area = "InsuranceContractBaseData" });
            ViewBag.layer = "_WebLayout";

            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست درانتظار دریافت اصل مدرک", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("InsuranceContractBaseData", "MyW8ForReciveingRealDocumentsFilledContract")));
        }

        [AreaConfig(Title = "مشاهده جززیات خسارت ثبت شده", Icon = "fa-eye")]
        [HttpGet]
        public IActionResult Detaile([FromQuery] long? id, [FromQuery] bool isPrint = false)
        {
            ViewBag.isPrint = isPrint;
            ViewBag.newLayoutName = "_WebLayout";

            return View("~/Views/Contract/Detaile.cshtml", InsuranceContractProposalFilledFormService.Detaile(id, HttpContext.GetLoginUser()?.UserId, SiteSettingService.GetSiteSetting()?.Id, false, validStatus));
        }

        [AreaConfig(Title = "دانلود پی دی اف فرم پیشنهاد", Icon = "fa-download")]
        [HttpGet]
        public IActionResult DownloadPdf([FromQuery] GlobalLongId input)
        {
            return File(
                    HtmlToPdfBlink.Convert((Request.IsHttps ? "https" : "http") + "://" + Request.Host + Url.Action("Detaile", "MyDefectiveDocumentFilledContract", new { area = "InsuranceContractBaseData", id = input.id, isPrint = true }), Request.Cookies),
                    System.Net.Mime.MediaTypeNames.Application.Pdf, DateTime.Now.ToFaDate("_") + "_" + DateTime.Now.ToString("HH_mm_ss") + ".pdf"
                );
        }

        [AreaConfig(Title = "مشاهده لیست درانتظار دریافت اصل مدرک", Icon = "fa-list-alt ")]
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
