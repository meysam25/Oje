using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.AccountService.Models.View;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.MessengerBaseData.Areas.MessengerBaseData.Controllers
{
    [Area("MessengerBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات پیام رسان", Icon = "fa-flag", Title = "تمپلیت نوتیفیکیشن")]
    [CustomeAuthorizeFilter]
    public class UserNotificationTemplateController: Controller
    {
        readonly IUserNotificationTemplateService UserNotificationTemplateService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public UserNotificationTemplateController(
                IUserNotificationTemplateService UserNotificationTemplateService,
                ISiteSettingService SiteSettingService
            )
        {
            this.UserNotificationTemplateService = UserNotificationTemplateService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "تمپلیت نوتیفیکیشن", Icon = "fa-eye", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "تمپلیت نوتیفیکیشن";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "UserNotificationTemplate", new { area = "MessengerBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست تمپلیت نوتیفیکیشن", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("MessengerBaseData", "UserNotificationTemplate")));
        }

        [AreaConfig(Title = "افزودن تمپلیت نوتیفیکیشن جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateUserNotificationTemplateVM input)
        {
            return Json(UserNotificationTemplateService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف تمپلیت نوتیفیکیشن", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(UserNotificationTemplateService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک تمپلیت نوتیفیکیشن", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(UserNotificationTemplateService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  تمپلیت نوتیفیکیشن", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateUserNotificationTemplateVM input)
        {
            return Json(UserNotificationTemplateService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست تمپلیت نوتیفیکیشن", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] UserNotificationTemplateMainGrid searchInput)
        {
            return Json(UserNotificationTemplateService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] UserNotificationTemplateMainGrid searchInput)
        {
            var result = UserNotificationTemplateService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
