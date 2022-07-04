using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.AccountService.Models.View;
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
    [AreaConfig(ModualTitle = "تنظیمات پیام رسان", Icon = "fa-flag", Title = "وب نوتیفیکیشن")]
    [CustomeAuthorizeFilter]
    public class ExternalNotificationServiceConfigController: Controller
    {
        readonly IExternalNotificationServiceConfigService ExternalNotificationServiceConfigService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public ExternalNotificationServiceConfigController
            (
                IExternalNotificationServiceConfigService ExternalNotificationServiceConfigService,
                ISiteSettingService SiteSettingService
            )
        {
            this.ExternalNotificationServiceConfigService = ExternalNotificationServiceConfigService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "وب نوتیفیکیشن", Icon = "fa-bell", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "وب نوتیفیکیشن";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ExternalNotificationServiceConfig", new { area = "MessengerBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست وب نوتیفیکیشن", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("MessengerBaseData", "ExternalNotificationServiceConfig")));
        }

        [AreaConfig(Title = "افزودن وب نوتیفیکیشن جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] ExternalNotificationServiceConfigCreateUpdateVM input)
        {
            return Json(ExternalNotificationServiceConfigService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف وب نوتیفیکیشن", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(ExternalNotificationServiceConfigService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک وب نوتیفیکیشن", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(ExternalNotificationServiceConfigService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  وب نوتیفیکیشن", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] ExternalNotificationServiceConfigCreateUpdateVM input)
        {
            return Json(ExternalNotificationServiceConfigService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست وب نوتیفیکیشن", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] ExternalNotificationServiceConfigMainGrid searchInput)
        {
            return Json(ExternalNotificationServiceConfigService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ExternalNotificationServiceConfigMainGrid searchInput)
        {
            var result = ExternalNotificationServiceConfigService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
