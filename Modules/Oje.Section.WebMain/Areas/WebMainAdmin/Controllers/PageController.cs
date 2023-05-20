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

namespace Oje.Section.WebMain.Areas.WebMainAdmin.Controllers
{
    [Area("WebMainAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات وب سایت", Icon = "fa-browser", Title = "صفحه ها")]
    [CustomeAuthorizeFilter]
    public class PageController : Controller
    {
        readonly ISiteSettingService SiteSettingService = null;
        readonly IPageService PageService = null;
        public PageController
            (
                ISiteSettingService SiteSettingService,
                IPageService PageService
            )
        {
            this.SiteSettingService = SiteSettingService;
            this.PageService = PageService;
        }

        [AreaConfig(Title = "صفحه ها", Icon = "fa-newspaper", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "صفحه ها";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "Page", new { area = "WebMainAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست صفحه ها", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("WebMainAdmin", "Page")));
        }

        [AreaConfig(Title = "افزودن صفحه ها جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] PageCreateUpdateVM input)
        {
            return Json(PageService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف صفحه ها", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(PageService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک صفحه ها", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(PageService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  صفحه ها", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] PageCreateUpdateVM input)
        {
            return Json(PageService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست صفحه ها", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] PageMainGrid searchInput)
        {
            return Json(PageService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] PageMainGrid searchInput)
        {
            var result = PageService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
