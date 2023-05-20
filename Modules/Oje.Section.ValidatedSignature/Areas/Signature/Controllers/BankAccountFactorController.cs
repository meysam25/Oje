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
    [AreaConfig(ModualTitle = "اعتبارسنجی امضا", Icon = "fa-spider-black-widow", Title = "فاکتور")]
    [CustomeAuthorizeFilter]
    public class BankAccountFactorController : Controller
    {
        readonly IBankAccountFactorService BankAccountFactorService = null;

        public BankAccountFactorController
            (
                IBankAccountFactorService BankAccountFactorService
            )
        {
            this.BankAccountFactorService = BankAccountFactorService;
        }

        [AreaConfig(Title = "فاکتور", Icon = "fa-usd-circle", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "فاکتور";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "BankAccountFactor", new { area = "Signature" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست فاکتور", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Signature", "BankAccountFactor")));
        }

        [AreaConfig(Title = "مشاهده جزئیات تغییرات غیر معتبر", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] string id)
        {
            return Json(BankAccountFactorService.GetBy(id));
        }

        [AreaConfig(Title = "مشاهده لیست فاکتور", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] BankAccountFactorMainGrid searchInput)
        {
            return Json(BankAccountFactorService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] BankAccountFactorMainGrid searchInput)
        {
            var result = BankAccountFactorService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
