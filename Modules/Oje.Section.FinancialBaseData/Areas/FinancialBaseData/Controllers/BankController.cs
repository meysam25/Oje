using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using Oje.PaymentService.Interfaces;
using Oje.PaymentService.Models.DB;
using Oje.PaymentService.Models.View;

namespace Oje.Section.FinancialBaseData.Areas.FinancialBaseData.Controllers
{
    [Area("FinancialBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "اطلاعات پایه مالی", Icon = "fa-dollar-sign", Title = "لیست بانک")]
    [CustomeAuthorizeFilter]
    public class BankController: Controller
    {
        readonly IBankService BankService = null;
        readonly IBankAccountSizpayPaymentService BankAccountSizpayPaymentService = null;
        public BankController(IBankService BankService, IBankAccountSizpayPaymentService BankAccountSizpayPaymentService)
        {
            this.BankService = BankService;
            this.BankAccountSizpayPaymentService = BankAccountSizpayPaymentService;
        }

        [AreaConfig(Title = "لیست بانک", Icon = "fa-university", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "لیست بانک";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "Bank", new { area = "FinancialBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست بانک", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("FinancialBaseData", "Bank")));
        }

        [AreaConfig(Title = "افزودن لیست بانک جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] BankCreateUpdateVM input)
        {
            return Json(BankService.Create(input, HttpContext.GetLoginUserId()?.UserId));
        }

        [AreaConfig(Title = "حذف لیست بانک", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(BankService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک لیست بانک", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(BankService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  لیست بانک", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] BankCreateUpdateVM input)
        {
            return Json(BankService.Update(input, HttpContext.GetLoginUserId()?.UserId));
        }

        [AreaConfig(Title = "مشاهده لیست لیست بانک", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] BankMainGrid searchInput)
        {
            return Json(BankService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] BankMainGrid searchInput)
        {
            var result = BankService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
