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
    [AreaConfig(ModualTitle = "تنظیمات پیام رسان", Icon = "fa-flag", Title = "تنظیمات تمپلیت ایمیل")]
    [CustomeAuthorizeFilter]
    public class EmailTemplateController: Controller
    {
        readonly IEmailTemplateService EmailTemplateService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public EmailTemplateController(IEmailTemplateService EmailTemplateService, ISiteSettingService SiteSettingService)
        {
            this.EmailTemplateService = EmailTemplateService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "تمپلیت ایمیل", Icon = "fa-eye", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "تمپلیت ایمیل";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "EmailTemplate", new { area = "MessengerBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست تمپلیت ایمیل", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("MessengerBaseData", "EmailTemplate")));
        }

        [AreaConfig(Title = "افزودن تمپلیت ایمیل جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] EmailTemplateCreateUpdateVM input)
        {
            return Json(EmailTemplateService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف تمپلیت ایمیل", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(EmailTemplateService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده یک تمپلیت ایمیل", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(EmailTemplateService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی تمپلیت ایمیل", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] EmailTemplateCreateUpdateVM input)
        {
            return Json(EmailTemplateService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست تمپلیت ایمیل", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] EmailTemplateMainGrid searchInput)
        {
            return Json(EmailTemplateService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] EmailTemplateMainGrid searchInput)
        {
            var result = EmailTemplateService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
