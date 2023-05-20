using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Services;
using System;
using Oje.ValidatedSignature.Interfaces;
using Oje.ValidatedSignature.Models.View;

namespace Oje.Section.ValidatedSignature.Areas.Signature.Controllers
{
    [Area("Signature")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "اعتبارسنجی امضا", Icon = "fa-spider-black-widow", Title = "حساب های بانکی")]
    [CustomeAuthorizeFilter]
    public class BankAccountController: Controller
    {
        readonly IBankAccountService BankAccountService = null;

        public BankAccountController
            (
                IBankAccountService BankAccountService
            )
        {
            this.BankAccountService = BankAccountService;
        }

        [AreaConfig(Title = "حساب های بانکی", Icon = "fa-usd-circle", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "حساب های بانکی";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "BankAccount", new { area = "Signature" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست حساب های بانکی", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Signature", "BankAccount")));
        }

        [AreaConfig(Title = "مشاهده جزئیات تغییرات غیر معتبر", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] long? id)
        {
            return Json(BankAccountService.GetBy(id));
        }

        [AreaConfig(Title = "مشاهده لیست حساب های بانکی", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] BankAccountMainGrid searchInput)
        {
            return Json(BankAccountService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] BankAccountMainGrid searchInput)
        {
            var result = BankAccountService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
