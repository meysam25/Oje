using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Security.Interfaces;
using Oje.Security.Models.View;
using System;

namespace Oje.Section.Security.Areas.Security.Controllers
{
    [Area("Security")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "امنیت", Icon = "fa-user-secret", Title = "قوانین محدود کردن کاربر ادمین")]
    [CustomeAuthorizeFilter]
    public class AdminBlockClientConfigController: Controller
    {
        readonly IAdminBlockClientConfigService AdminBlockClientConfigService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly Oje.Security.Interfaces.IActionService ActionService = null;
        public AdminBlockClientConfigController
            (
                IAdminBlockClientConfigService AdminBlockClientConfigService,
                ISiteSettingService SiteSettingService,
                Oje.Security.Interfaces.IActionService ActionService
            )
        {
            this.AdminBlockClientConfigService = AdminBlockClientConfigService;
            this.SiteSettingService = SiteSettingService;
            this.ActionService = ActionService;
        }

        [AreaConfig(Title = "قوانین محدود کردن کاربر ادمین", Icon = "fa-shield-check", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "قوانین محدود کردن کاربر ادمین";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "AdminBlockClientConfig", new { area = "Security" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست قوانین محدود کردن کاربر ادمین", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Security", "AdminBlockClientConfig")));
        }

        [AreaConfig(Title = "افزودن قوانین محدود کردن کاربر ادمین جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] AdminBlockClientConfigCreateUpdateVM input)
        {
            return Json(AdminBlockClientConfigService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف قوانین محدود کردن کاربر ادمین", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(AdminBlockClientConfigService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک قوانین محدود کردن کاربر ادمین", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(AdminBlockClientConfigService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  قوانین محدود کردن کاربر ادمین", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] AdminBlockClientConfigCreateUpdateVM input)
        {
            return Json(AdminBlockClientConfigService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست قوانین محدود کردن کاربر ادمین", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] AdminBlockClientConfigMainGrid searchInput)
        {
            return Json(AdminBlockClientConfigService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] AdminBlockClientConfigMainGrid searchInput)
        {
            var result = AdminBlockClientConfigService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
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
