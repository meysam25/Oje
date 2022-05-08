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
    [AreaConfig(ModualTitle = "مدیریت قرارداد ها و مجوز ها", Icon = "fa-file-invoice", Title = "خسارت های رفع نقص شده برای بازبینی")]
    [CustomeAuthorizeFilter]
    public class InsuranceContractProposalFilledFormReviewedController : Controller
    {
        readonly IInsuranceContractProposalFilledFormService InsuranceContractProposalFilledFormService = null;
        readonly IInsuranceContractProposalFilledFormStatusLogService InsuranceContractProposalFilledFormStatusLogService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly InsuranceContractProposalFilledFormType status = InsuranceContractProposalFilledFormType.Reviewed;

        public InsuranceContractProposalFilledFormReviewedController
            (
                IInsuranceContractProposalFilledFormService InsuranceContractProposalFilledFormService,
                ISiteSettingService SiteSettingService,
                IInsuranceContractProposalFilledFormStatusLogService InsuranceContractProposalFilledFormStatusLogService
            )
        {
            this.InsuranceContractProposalFilledFormService = InsuranceContractProposalFilledFormService;
            this.SiteSettingService = SiteSettingService;
            this.InsuranceContractProposalFilledFormStatusLogService = InsuranceContractProposalFilledFormStatusLogService;
        }

        [AreaConfig(Title = "خسارت های رفع نقص شده برای بازبینی", Icon = "fa-print-search", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "خسارت های رفع نقص شده برای بازبینی";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "InsuranceContractProposalFilledFormReviewed", new { area = "InsuranceContractBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست خسارت های رفع نقص شده برای بازبینی", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("InsuranceContractBaseData", "InsuranceContractProposalFilledFormReviewed")));
        }

        [AreaConfig(Title = "حذف خسارت های رفع نقص شده برای بازبینی", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(InsuranceContractProposalFilledFormService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id, status));
        }

        [AreaConfig(Title = "مشاهده جززیات خسارت ثبت شده", Icon = "fa-eye")]
        [HttpGet]
        public IActionResult Detaile([FromQuery] long? id, [FromQuery] bool isPrint = false)
        {
            ViewBag.isPrint = isPrint;
            return View("~/Views/Contract/Detaile.cshtml", InsuranceContractProposalFilledFormService.Detaile(id, HttpContext.GetLoginUser()?.UserId, SiteSettingService.GetSiteSetting()?.Id, true, new List<InsuranceContractProposalFilledFormType>() { status }));
        }

        [AreaConfig(Title = "مشاهده وضعیت فرم پیشنهاد تایید شده", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetStatus([FromForm] GlobalLongId input)
        {
            return Json(InsuranceContractProposalFilledFormService.GetStatus(input?.id, SiteSettingService.GetSiteSetting()?.Id, status));
        }

        [AreaConfig(Title = "تغییر وضعیت فرم پیشنهاد", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult UpdateStatus([FromForm] InsuranceContractProposalFilledFormChangeStatusVM input)
        {
            return Json(InsuranceContractProposalFilledFormService.UpdateStatus(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, status));
        }

        [AreaConfig(Title = "مشاهده قیمت تایین شده فرم پیشنهاد", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetPrice([FromForm] GlobalLongId input)
        {
            return Json(InsuranceContractProposalFilledFormService.GetPrice(input?.id, SiteSettingService.GetSiteSetting()?.Id, status));
        }

        [AreaConfig(Title = "تغییر قیمت فرم پیشنهاد", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult UpdatePrice([FromForm] InsuranceContractProposalFilledFormChangePriceVM input)
        {
            return Json(InsuranceContractProposalFilledFormService.UpdatePrice(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, status));
        }

        [AreaConfig(Title = "دانلود پی دی اف فرم پیشنهاد", Icon = "fa-download")]
        [HttpGet]
        public IActionResult DownloadPdf([FromQuery] GlobalLongId input)
        {
            return File(
                    HtmlToPdfBlink.Convert((Request.IsHttps ? "https" : "http") + "://" + Request.Host + Url.Action("Detaile", "InsuranceContractProposalFilledForm", new { area = "InsuranceContractBaseData", id = input.id, isPrint = true }), Request.Cookies),
                    System.Net.Mime.MediaTypeNames.Application.Pdf, DateTime.Now.ToFaDate("_") + "_" + DateTime.Now.ToString("HH_mm_ss") + ".pdf"
                );
        }

        [AreaConfig(Title = "مشاهده لیست خسارت های رفع نقص شده برای بازبینی", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] InsuranceContractProposalFilledFormMainGrid searchInput)
        {
            return Json(InsuranceContractProposalFilledFormService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, status));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] InsuranceContractProposalFilledFormMainGrid searchInput)
        {
            var result = InsuranceContractProposalFilledFormService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, status);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست تاریخچه خسارت ثبت شده", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetStatusLogList([FromForm] InsuranceContractProposalFilledFormStatusLogGrid searchInput)
        {
            return Json(InsuranceContractProposalFilledFormStatusLogService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, status));
        }

        [AreaConfig(Title = "مشاهده مدارک لیست خسارت های رفع نقص شده برای بازبینی", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetPPFImageList([FromForm] GlobalGridParentLong input)
        {
            return Json(InsuranceContractProposalFilledFormService.GetPPFImageList(input, SiteSettingService.GetSiteSetting()?.Id, status));
        }
    }
}
