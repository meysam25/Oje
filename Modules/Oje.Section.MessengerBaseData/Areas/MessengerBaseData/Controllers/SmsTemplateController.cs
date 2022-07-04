using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Sms.Interfaces;
using Oje.Sms.Models.View;
using System;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.MessengerBaseData.Areas.MessengerBaseData.Controllers
{
    [Area("MessengerBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات پیام رسان", Icon = "fa-flag", Title = "تنظیمات تمپلیت پیامک")]
    [CustomeAuthorizeFilter]
    public class SmsTemplateController: Controller
    {
        readonly ISmsTemplateService SmsTemplateService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public SmsTemplateController(ISmsTemplateService SmsTemplateService, ISiteSettingService SiteSettingService)
        {
            this.SmsTemplateService = SmsTemplateService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "تمپلیت پیامک", Icon = "fa-eye", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "تمپلیت پیامک";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "SmsTemplate", new { area = "MessengerBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست تمپلیت پیامک", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("MessengerBaseData", "SmsTemplate")));
        }

        [AreaConfig(Title = "افزودن تمپلیت پیامک جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateSmsTemplateVM input)
        {
            return Json(SmsTemplateService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف تمپلیت پیامک", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(SmsTemplateService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده یک تمپلیت پیامک", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(SmsTemplateService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی تمپلیت پیامک", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateSmsTemplateVM input)
        {
            return Json(SmsTemplateService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست تمپلیت پیامک", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] SmsTemplateMainGrid searchInput)
        {
            return Json(SmsTemplateService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] SmsTemplateMainGrid searchInput)
        {
            var result = SmsTemplateService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
