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
    [AreaConfig(ModualTitle = "اعتبارسنجی امضا", Icon = "fa-spider-black-widow", Title = "کیف پول کاربر")]
    [CustomeAuthorizeFilter]
    public class WalletTransactionController: Controller
    {
        readonly IWalletTransactionService WalletTransactionService = null;

        public WalletTransactionController (IWalletTransactionService WalletTransactionService)
        {
            this.WalletTransactionService = WalletTransactionService;
        }

        [AreaConfig(Title = "کیف پول کاربر", Icon = "fa-check-circle", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "کیف پول کاربر";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "WalletTransaction", new { area = "Signature" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست کیف پول کاربر", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Signature", "WalletTransaction")));
        }

        [AreaConfig(Title = "مشاهده جزئیات تغییرات غیر معتبر", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] long? id)
        {
            return Json(WalletTransactionService.GetBy(id));
        }

        [AreaConfig(Title = "مشاهده لیست کیف پول کاربر", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] WalletTransactionMainGrid searchInput)
        {
            return Json(WalletTransactionService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] WalletTransactionMainGrid searchInput)
        {
            var result = WalletTransactionService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
