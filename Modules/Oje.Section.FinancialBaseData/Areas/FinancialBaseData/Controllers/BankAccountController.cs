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

namespace Oje.Section.FinancialBaseData.Areas.FinancialBaseData.Controllers
{
    [Area("FinancialBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "اطلاعات پایه مالی", Icon = "fa-dollar-sign", Title = "حساب بانکی")]
    [CustomeAuthorizeFilter]
    public class BankAccountController : Controller
    {
        readonly IBankAccountService BankAccountService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IBankService BankService = null;
        readonly AccountService.Interfaces.IUserService UserService = null;
        public BankAccountController
            (
                IBankAccountService BankAccountService,
                ISiteSettingService SiteSettingService,
                IBankService BankService,
                AccountService.Interfaces.IUserService UserService
            )
        {
            this.BankAccountService = BankAccountService;
            this.SiteSettingService = SiteSettingService;
            this.BankService = BankService;
            this.UserService = UserService;
        }

        [AreaConfig(Title = "حساب بانکی", Icon = "fa-credit-card", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "حساب بانکی";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "BankAccount", new { area = "FinancialBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه حساب بانکی", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("FinancialBaseData", "BankAccount")));
        }

        [AreaConfig(Title = "افزودن حساب بانکی جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] BankAccountCreateUpdateVM input)
        {
            return Json(BankAccountService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف حساب بانکی", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(BankAccountService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک حساب بانکی", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(BankAccountService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  حساب بانکی", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] BankAccountCreateUpdateVM input)
        {
            return Json(BankAccountService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست حساب بانکی", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] BankAccountMainGrid searchInput)
        {
            return Json(BankAccountService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] BankAccountMainGrid searchInput)
        {
            var result = BankAccountService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست بانک", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetBankList()
        {
            return Json(BankService.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست کاربران", Icon = "fa-list-alt ")]
        [HttpGet]
        public ActionResult GetUsers([FromQuery] Select2SearchVM searchInput)
        {
            return Json(UserService.GetSelect2List(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
