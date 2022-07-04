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
    [AreaConfig(ModualTitle = "وب سایت", Icon = "fa-browser", Title = "جزییات دیزاین چپ و راست")]
    [CustomeAuthorizeFilter]
    public class PageLeftRightDesignItemController: Controller
    {
        readonly IPageLeftRightDesignItemService PageLeftRightDesignItemService = null;
        readonly IPageLeftRightDesignService PageLeftRightDesignService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public PageLeftRightDesignItemController
            (
                IPageLeftRightDesignItemService PageLeftRightDesignItemService,
                ISiteSettingService SiteSettingService,
                IPageLeftRightDesignService PageLeftRightDesignService
            )
        {
            this.PageLeftRightDesignItemService = PageLeftRightDesignItemService;
            this.SiteSettingService = SiteSettingService;
            this.PageLeftRightDesignService = PageLeftRightDesignService;
        }

        [AreaConfig(Title = "جزییات دیزاین چپ و راست", Icon = "fa-palette", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "جزییات دیزاین چپ و راست";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "PageLeftRightDesignItem", new { area = "WebMainAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست جزییات دیزاین چپ و راست", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("WebMainAdmin", "PageLeftRightDesignItem")));
        }

        [AreaConfig(Title = "افزودن جزییات دیزاین چپ و راست جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] PageLeftRightDesignItemCreateUpdateVM input)
        {
            return Json(PageLeftRightDesignItemService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف جزییات دیزاین چپ و راست", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(PageLeftRightDesignItemService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک جزییات دیزاین چپ و راست", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalLongId input)
        {
            return Json(PageLeftRightDesignItemService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  جزییات دیزاین چپ و راست", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] PageLeftRightDesignItemCreateUpdateVM input)
        {
            return Json(PageLeftRightDesignItemService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست جزییات دیزاین چپ و راست", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] PageLeftRightDesignItemMainGrid searchInput)
        {
            return Json(PageLeftRightDesignItemService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] PageLeftRightDesignItemMainGrid searchInput)
        {
            var result = PageLeftRightDesignItemService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست صفحه ها", Icon = "fa-list-alt ")]
        [HttpGet]
        public ActionResult GetPageLightList([FromForm] Select2SearchVM searchInput)
        {
            return Json(PageLeftRightDesignService.GetSelect2(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
