using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.WebMain.Interfaces;
using Oje.Section.WebMain.Models.View;
using System;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.WebMain.Areas.Controllers
{
    [Area("WebMainAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "وب سایت", Icon = "fa-browser", Title = "منوی بالا")]
    [CustomeAuthorizeFilter]
    public class TopMenuController: Controller
    {
        readonly ITopMenuService TopMenuService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public TopMenuController
            (
                ITopMenuService TopMenuService,
                ISiteSettingService SiteSettingService
            )
        {
            this.TopMenuService = TopMenuService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "منوی بالا", Icon = "fa-bars", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "منوی بالا";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "TopMenu", new { area = "WebMainAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست منوی بالا", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("WebMainAdmin", "TopMenu")));
        }

        [AreaConfig(Title = "افزودن منوی بالا جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] TopMenuCreateUpdateVM input)
        {
            return Json(TopMenuService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف منوی بالا", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(TopMenuService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک منوی بالا", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalLongId input)
        {
            return Json(TopMenuService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  منوی بالا", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] TopMenuCreateUpdateVM input)
        {
            return Json(TopMenuService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست منوی بالا", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] TopMenuMainGrid searchInput)
        {
            return Json(TopMenuService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] TopMenuMainGrid searchInput)
        {
            var result = TopMenuService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
