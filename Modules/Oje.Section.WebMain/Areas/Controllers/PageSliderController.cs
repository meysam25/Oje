using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Exceptions;
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
    [AreaConfig(ModualTitle = "وب سایت", Icon = "fa-browser", Title = "اسلایدر صفحه ها")]
    [CustomeAuthorizeFilter]
    public class PageSliderController: Controller
    {
        readonly IPageSliderService PageSliderService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IPageService PageService = null;

        public PageSliderController
            (
                IPageSliderService PageSliderService,
                ISiteSettingService SiteSettingService,
                IPageService PageService
            )
        {
            this.PageSliderService = PageSliderService;
            this.SiteSettingService = SiteSettingService;
            this.PageService = PageService;
        }

        [AreaConfig(Title = "اسلاید صفحه ها", Icon = "fa-presentation", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "اسلاید صفحه ها";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "PageSlider", new { area = "WebMainAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست اسلاید صفحه ها", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("WebMainAdmin", "PageSlider")));
        }

        [AreaConfig(Title = "افزودن اسلاید صفحه ها جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] PageSliderCreateUpdateVM input)
        {
            return Json(PageSliderService.Create(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId));
        }

        [AreaConfig(Title = "حذف اسلاید صفحه ها", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(PageSliderService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک اسلاید صفحه ها", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(PageSliderService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  اسلاید صفحه ها", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] PageSliderCreateUpdateVM input)
        {
            return Json(PageSliderService.Update(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId));
        }

        [AreaConfig(Title = "مشاهده لیست اسلاید صفحه ها", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] PageSliderMainGrid searchInput)
        {
            return Json(PageSliderService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] PageSliderMainGrid searchInput)
        {
            var result = PageSliderService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست صفحه ها", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetPageLightList([FromForm] Select2SearchVM searchInput)
        {
            return Json(PageService.GetSelect2(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
