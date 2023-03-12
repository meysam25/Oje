using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Services;
using System;
using Oje.ValidatedSignature.Models.View;
using Oje.ValidatedSignature.Interfaces;

namespace Oje.Section.ValidatedSignature.Areas.Signature.Controllers
{
    [Area("Signature")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "اعتبارسنجی امضا", Icon = "fa-spider-black-widow", Title = "کد های پیامکی")]
    [CustomeAuthorizeFilter]
    public class SmsValidationHistoryController: Controller
    {
        readonly ISmsValidationHistoryService SmsValidationHistoryService = null;

        public SmsValidationHistoryController
            (
                ISmsValidationHistoryService SmsValidationHistoryService
            )
        {
            this.SmsValidationHistoryService = SmsValidationHistoryService;
        }

        [AreaConfig(Title = "کد های پیامکی", Icon = "fa-check-circle", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "کد های پیامکی";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "SmsValidationHistory", new { area = "Signature" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست کد های پیامکی", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Signature", "SmsValidationHistory")));
        }

        [AreaConfig(Title = "مشاهده جزئیات تغییرات غیر معتبر", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] string id)
        {
            return Json(SmsValidationHistoryService.GetBy(id));
        }

        [AreaConfig(Title = "مشاهده لیست کد های پیامکی", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] SmsValidationHistoryMainGrid searchInput)
        {
            return Json(SmsValidationHistoryService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] SmsValidationHistoryMainGrid searchInput)
        {
            var result = SmsValidationHistoryService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
