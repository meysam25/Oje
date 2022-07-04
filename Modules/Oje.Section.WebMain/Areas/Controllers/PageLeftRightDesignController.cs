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
    [AreaConfig(ModualTitle = "وب سایت", Icon = "fa-browser", Title = "دیزاین چپ و راست")]
    [CustomeAuthorizeFilter]
    public class PageLeftRightDesignController : Controller
    {
        readonly IPageLeftRightDesignService PageLeftRightDesignService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IPageService PageService = null;

        public PageLeftRightDesignController
            (
                IPageLeftRightDesignService PageLeftRightDesignService,
                ISiteSettingService SiteSettingService,
                IPageService PageService
            )
        {
            this.PageLeftRightDesignService = PageLeftRightDesignService;
            this.SiteSettingService = SiteSettingService;
            this.PageService = PageService;
        }

        [AreaConfig(Title = "دیزاین چپ و راست", Icon = "fa-palette", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "دیزاین چپ و راست";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "PageLeftRightDesign", new { area = "WebMainAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست دیزاین چپ و راست", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("WebMainAdmin", "PageLeftRightDesign")));
        }

        [AreaConfig(Title = "افزودن دیزاین چپ و راست جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] PageLeftRightDesignCreateUpdateVM input)
        {
            return Json(PageLeftRightDesignService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف دیزاین چپ و راست", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(PageLeftRightDesignService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک دیزاین چپ و راست", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(PageLeftRightDesignService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  دیزاین چپ و راست", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] PageLeftRightDesignCreateUpdateVM input)
        {
            return Json(PageLeftRightDesignService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست دیزاین چپ و راست", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] PageLeftRightDesignMainGrid searchInput)
        {
            return Json(PageLeftRightDesignService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] PageLeftRightDesignMainGrid searchInput)
        {
            var result = PageLeftRightDesignService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
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
            return Json(PageService.GetSelect2(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
