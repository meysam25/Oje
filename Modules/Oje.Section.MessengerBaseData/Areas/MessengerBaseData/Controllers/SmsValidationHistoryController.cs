using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Services;
using Oje.Sms.Interfaces;
using Oje.Sms.Models.View;
using System;

namespace Oje.Section.MessengerBaseData.Areas.MessengerBaseData.Controllers
{
    [Area("MessengerBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات پیام رسان", Icon = "fa-flag", Title = "لاگ کد پیامک شده")]
    [CustomeAuthorizeFilter]
    public class SmsValidationHistoryController: Controller
    {
        readonly ISmsValidationHistoryService SmsValidationHistoryService = null;
        readonly ISiteSettingService SiteSettingService = null;

        public SmsValidationHistoryController
            (
                ISmsValidationHistoryService SmsValidationHistoryService, 
                ISiteSettingService SiteSettingService
            )
        {
            this.SmsValidationHistoryService = SmsValidationHistoryService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "لاگ کد پیامک شده", Icon = "fa-bug", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "لاگ کد پیامک شده";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "SmsValidationHistory", new { area = "MessengerBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست لاگ کد پیامک شده", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("MessengerBaseData", "SmsValidationHistory")));
        }

        [AreaConfig(Title = "مشاهده لیست لاگ کد پیامک شده", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] SmsValidationHistoryMainGrid searchInput)
        {
            return Json(SmsValidationHistoryService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] SmsValidationHistoryMainGrid searchInput)
        {
            var result = SmsValidationHistoryService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
