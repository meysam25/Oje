using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.PaymentService.Interfaces;
using Oje.PaymentService.Models.View;
using System;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.FinancialBaseData.Areas.FinancialBaseData.Controllers
{
    [Area("FinancialBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "اطلاعات پایه مالی", Icon = "fa-dollar-sign", Title = "لیست سیزپی")]
    [CustomeAuthorizeFilter]
    public class BankAccountSizpayController: Controller
    {
        readonly IBankAccountSizpayService BankAccountSizpayService = null;
        readonly IBankAccountService BankAccountService = null;
        readonly ISiteSettingService SiteSettingService = null;

        public BankAccountSizpayController
            (
                IBankAccountSizpayService BankAccountSizpayService,
                IBankAccountService BankAccountService,
                ISiteSettingService SiteSettingService
            )
        {
            this.BankAccountSizpayService = BankAccountSizpayService;
            this.BankAccountService = BankAccountService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "لیست سیزپی", Icon = "fa-dollar-sign", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "لیست سیزپی";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "BankAccountSizpay", new { area = "FinancialBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست سیزپی", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("FinancialBaseData", "BankAccountSizpay")));
        }

        [AreaConfig(Title = "افزودن لیست سیزپی جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] BankAccountSizpayCreateUpdateVM input)
        {
            return Json(BankAccountSizpayService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف لیست سیزپی", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(BankAccountSizpayService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک لیست سیزپی", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(BankAccountSizpayService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  لیست سیزپی", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] BankAccountSizpayCreateUpdateVM input)
        {
            return Json(BankAccountSizpayService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست لیست سیزپی", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] BankAccountSizpayMainGrid searchInput)
        {
            return Json(BankAccountSizpayService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] BankAccountSizpayMainGrid searchInput)
        {
            var result = BankAccountSizpayService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست حساب", Icon = "fa-list-alt ")]
        [HttpGet]
        public ActionResult GetBankAccounts([FromQuery] Select2SearchVM searchInput, [FromQuery] int? cSOWSiteSettingId)
        {
            return Json(BankAccountService.GetSelect2List(searchInput, HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
