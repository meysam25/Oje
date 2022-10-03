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
    [AreaConfig(ModualTitle = "اطلاعات پایه مالی", Icon = "fa-dollar-sign", Title = "لیست سداد")]
    [CustomeAuthorizeFilter]
    public class BankAccountSadadController: Controller
    {
        readonly IBankAccountSadadService BankAccountSadadService = null;
        readonly IBankAccountService BankAccountService = null;
        readonly ISiteSettingService SiteSettingService = null;

        public BankAccountSadadController
            (
                IBankAccountSadadService BankAccountSadadService,
                IBankAccountService BankAccountService,
                ISiteSettingService SiteSettingService
            )
        {
            this.BankAccountSadadService = BankAccountSadadService;
            this.BankAccountService = BankAccountService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "لیست سداد", Icon = "fa-dollar-sign", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "لیست سداد";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "BankAccountSadad", new { area = "FinancialBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست سداد", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("FinancialBaseData", "BankAccountSadad")));
        }

        [AreaConfig(Title = "افزودن لیست سداد جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] BankAccountSadadCreateUpdateVM input)
        {
            return Json(BankAccountSadadService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف لیست سداد", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(BankAccountSadadService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک لیست سداد", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(BankAccountSadadService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  لیست سداد", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] BankAccountSadadCreateUpdateVM input)
        {
            return Json(BankAccountSadadService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست لیست سداد", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] BankAccountSadadMainGrid searchInput)
        {
            return Json(BankAccountSadadService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] BankAccountSadadMainGrid searchInput)
        {
            var result = BankAccountSadadService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
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
