using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.EmailService.Interfaces;
using Oje.EmailService.Models.View;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using System;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.MessengerBaseData.Areas.MessengerBaseData.Controllers
{
    [Area("MessengerBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات پیام رسان", Icon = "fa-flag", Title = "تنظیمات ایمیل")]
    [CustomeAuthorizeFilter]
    public class EmailConfigController: Controller
    {
        readonly IEmailConfigService EmailConfigService = null;
        readonly ISiteSettingService SiteSettingService = null;

        public EmailConfigController(IEmailConfigService EmailConfigService, ISiteSettingService SiteSettingService)
        {
            this.EmailConfigService = EmailConfigService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "تنظیمات ایمیل", Icon = "fa-envelope", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "تنظیمات ایمیل";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "EmailConfig", new { area = "MessengerBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست تنظیمات ایمیل", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("MessengerBaseData", "EmailConfig")));
        }

        [AreaConfig(Title = "افزودن تنظیمات ایمیل جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] EmailConfigCreateUpdateVM input)
        {
            return Json(EmailConfigService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف تنظیمات ایمیل", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(EmailConfigService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک تنظیمات ایمیل", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(EmailConfigService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  تنظیمات ایمیل", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] EmailConfigCreateUpdateVM input)
        {
            return Json(EmailConfigService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست تنظیمات ایمیل", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] EmailConfigMainGrid searchInput)
        {
            return Json(EmailConfigService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] EmailConfigMainGrid searchInput)
        {
            var result = EmailConfigService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
