using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Filters;
using Oje.Section.WebMain.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Areas.Controllers
{
    [Area("WebMainAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "وب سایت", Icon = "fa-browser", Title = "یادآوری صفحه اصلی")]
    [CustomeAuthorizeFilter]
    public class ReminderMainPageController: Controller
    {
        readonly IPropertyService PropertyService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public ReminderMainPageController
            (
                IPropertyService PropertyService,
                ISiteSettingService SiteSettingService
            )
        {
            this.PropertyService = PropertyService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "یادآوری صفحه اصلی", Icon = "fa-calendar-week", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "یادآوری صفحه اصلی";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ReminderMainPage", new { area = "WebMainAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه یادآوری صفحه اصلی", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("WebMainAdmin", "ReminderMainPage")));
        }

        [AreaConfig(Title = "افزودن / به روز رسانی یادآوری صفحه اصلی", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult CreateUpdate([FromForm] ReminderMainPageCreateUpdateVM input)
        {
            return Json(PropertyService.CreateUpdate(input, SiteSettingService.GetSiteSetting()?.Id, PropertyType.RemindUsMainPage));
        }

        [AreaConfig(Title = "مشاهده یادآوری صفحه اصلی", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult Get()
        {
            return Json(PropertyService.GetBy<ReminderMainPageVM>(PropertyType.RemindUsMainPage, SiteSettingService.GetSiteSetting()?.Id));

        }
    }
}
