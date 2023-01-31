using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Security.Interfaces;
using Oje.Security.Models.View;
using System;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.Security.Areas.Security.Controllers
{
    [Area("Security")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "امنیت", Icon = "fa-user-secret", Title = "تنظیمات لاگ بخش ادمین")]
    [CustomeAuthorizeFilter]
    public class UserAdminLogConfigController: Controller
    {
        readonly IUserAdminLogConfigService UserAdminLogConfigService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly Oje.Security.Interfaces.IActionService ActionService = null;

        public UserAdminLogConfigController
            (
                IUserAdminLogConfigService UserAdminLogConfigService,
                ISiteSettingService SiteSettingService,
                Oje.Security.Interfaces.IActionService ActionService
            )
        {
            this.UserAdminLogConfigService = UserAdminLogConfigService;
            this.SiteSettingService = SiteSettingService;
            this.ActionService = ActionService;
        }

        [AreaConfig(Title = "تنظیمات لاگ بخش ادمین", Icon = "fa-user-secret", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "تنظیمات لاگ بخش ادمین";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "UserAdminLogConfig", new { area = "Security" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست تنظیمات لاگ بخش ادمین", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Security", "UserAdminLogConfig")));
        }

        [AreaConfig(Title = "افزودن تنظیمات لاگ بخش ادمین جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] UserAdminLogConfigCreateUpdateVM input)
        {
            return Json(UserAdminLogConfigService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "افزودن کلی تنظیمات لاگ بخش ادمین جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult AddAllActions()
        {
            return Json(UserAdminLogConfigService.CreateAll(SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف تنظیمات لاگ بخش ادمین", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(UserAdminLogConfigService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک تنظیمات لاگ بخش ادمین", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalLongId input)
        {
            return Json(UserAdminLogConfigService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  تنظیمات لاگ بخش ادمین", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] UserAdminLogConfigCreateUpdateVM input)
        {
            return Json(UserAdminLogConfigService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست تنظیمات لاگ بخش ادمین", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] UserAdminLogConfigMainGrid searchInput)
        {
            return Json(UserAdminLogConfigService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] UserAdminLogConfigMainGrid searchInput)
        {
            var result = UserAdminLogConfigService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [HttpGet]
        [AreaConfig(Title = "لیست بخش ها", Icon = "fa-list-alt")]
        public IActionResult GetSectionList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(ActionService.GetSelect2List(searchInput));
        }
    }
}
