using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.PaymentService.Interfaces;
using Oje.PaymentService.Models.View;
using System;

namespace Oje.Section.FinancialBaseData.Areas.FinancialBaseData.Controllers
{
    [Area("FinancialBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "اطلاعات پایه مالی", Icon = "fa-dollar-sign", Title = "لیست سپ")]
    [CustomeAuthorizeFilter]
    public class BankAccountSepController: Controller
    {
        readonly IBankAccountSepService BankAccountSepService = null;
        readonly IBankAccountService BankAccountService = null;
        readonly ISiteSettingService SiteSettingService = null;

        public BankAccountSepController
            (
                IBankAccountSepService BankAccountSepService,
                IBankAccountService BankAccountService,
                ISiteSettingService SiteSettingService
            )
        {
            this.BankAccountSepService = BankAccountSepService;
            this.BankAccountService = BankAccountService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "لیست سپ", Icon = "fa-dollar-sign", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "لیست سپ";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "BankAccountSep", new { area = "FinancialBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست سپ", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("FinancialBaseData", "BankAccountSep")));
        }

        [AreaConfig(Title = "افزودن لیست سپ جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] BankAccountSepCreateUpdateVM input)
        {
            return Json(BankAccountSepService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف لیست سپ", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(BankAccountSepService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک لیست سپ", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(BankAccountSepService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  لیست سپ", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] BankAccountSepCreateUpdateVM input)
        {
            return Json(BankAccountSepService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست لیست سپ", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] BankAccountSepMainGrid searchInput)
        {
            return Json(BankAccountSepService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] BankAccountSepMainGrid searchInput)
        {
            var result = BankAccountSepService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست حساب", Icon = "fa-list-alt ")]
        [HttpGet]
        public ActionResult GetBankAccounts([FromQuery] Select2SearchVM searchInput)
        {
            return Json(BankAccountService.GetSelect2List(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
