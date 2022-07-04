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
    [AreaConfig(ModualTitle = "وب سایت", Icon = "fa-browser", Title = "لینک اظافی فوتر")]
    [CustomeAuthorizeFilter]
    public class FooterExteraLinkController: Controller
    {
        readonly ISiteSettingService SiteSettingService = null;
        readonly IFooterExteraLinkService FooterExteraLinkService = null;
        public FooterExteraLinkController
            (
                ISiteSettingService SiteSettingService,
                IFooterExteraLinkService FooterExteraLinkService
            )
        {
            this.SiteSettingService = SiteSettingService;
            this.FooterExteraLinkService = FooterExteraLinkService;
        }

        [AreaConfig(Title = "لینک اظافی فوتر", Icon = "fa-link", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "لینک اظافی فوتر";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "FooterExteraLink", new { area = "WebMainAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست لینک اظافی فوتر", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("WebMainAdmin", "FooterExteraLink")));
        }

        [AreaConfig(Title = "افزودن لینک اظافی فوتر جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] FooterExteraLinkCreateUpdateVM input)
        {
            return Json(FooterExteraLinkService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف لینک اظافی فوتر", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(FooterExteraLinkService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک لینک اظافی فوتر", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(FooterExteraLinkService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  لینک اظافی فوتر", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] FooterExteraLinkCreateUpdateVM input)
        {
            return Json(FooterExteraLinkService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست لینک اظافی فوتر", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] FooterExteraLinkMainGrid searchInput)
        {
            return Json(FooterExteraLinkService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] FooterExteraLinkMainGrid searchInput)
        {
            var result = FooterExteraLinkService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
