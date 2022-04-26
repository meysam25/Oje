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

namespace Oje.Section.WebMain.Areas.Controllers
{
    [Area("WebMainAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "وب سایت", Icon = "fa-browser", Title = "جزییات معرفی صفحه")]
    [CustomeAuthorizeFilter]
    public class PageManifestItemController: Controller
    {
        readonly IPageManifestItemService PageManifestItemService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IPageManifestService PageManifestService = null;

        public PageManifestItemController
            (
                IPageManifestItemService PageManifestItemService,
                ISiteSettingService SiteSettingService,
                IPageManifestService PageManifestService
            )
        {
            this.PageManifestItemService = PageManifestItemService;
            this.SiteSettingService = SiteSettingService;
            this.PageManifestService = PageManifestService;
        }

        [AreaConfig(Title = "جزییات معرفی صفحه", Icon = "fa-palette", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "جزییات معرفی صفحه";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "PageManifestItem", new { area = "WebMainAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست جزییات معرفی صفحه", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("WebMainAdmin", "PageManifestItem")));
        }

        [AreaConfig(Title = "افزودن جزییات معرفی صفحه جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] PageManifestItemCreateUpdateVM input)
        {
            return Json(PageManifestItemService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف جزییات معرفی صفحه", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(PageManifestItemService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک جزییات معرفی صفحه", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalLongId input)
        {
            return Json(PageManifestItemService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  جزییات معرفی صفحه", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] PageManifestItemCreateUpdateVM input)
        {
            return Json(PageManifestItemService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست جزییات معرفی صفحه", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] PageManifestItemMainGrid searchInput)
        {
            return Json(PageManifestItemService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] PageManifestItemMainGrid searchInput)
        {
            var result = PageManifestItemService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست فهرست صفحه ها", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetPageLightList([FromForm] Select2SearchVM searchInput)
        {
            return Json(PageManifestService.GetSelect2(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
