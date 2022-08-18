using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Sanab.Interfaces;
using Oje.Sanab.Models.View;
using System;

namespace Oje.Section.Sanab.Areas.SanabAdmin.Controllers
{
    [Area("SanabAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "سنحاب", Icon = "fa-cctv", Title = "نام کاربری")]
    [CustomeAuthorizeFilter]
    public class SanabUserController: Controller
    {
        readonly ISanabUserService SanabUserService = null;
        readonly ISiteSettingService SiteSettingService = null;

        public SanabUserController
            (
                ISanabUserService SanabUserService,
                ISiteSettingService SiteSettingService
            )
        {
            this.SanabUserService = SanabUserService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "نام کاربری", Icon = "fa-user", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "نام کاربری";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "SanabUser", new { area = "SanabAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه نام کاربری", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("SanabAdmin", "SanabUser")));
        }

        [AreaConfig(Title = "افزودن یا ویرایش نام کاربری", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult CreateUpdate([FromForm] SanabUserCreateUpdateVM input)
        {
            return Json(SanabUserService.CreateUpdate(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده یک نام کاربری", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult Get()
        {
            return Json(SanabUserService.GetById(SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
