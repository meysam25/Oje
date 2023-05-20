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
    [AreaConfig(ModualTitle = "تنظیمات وب سایت", Icon = "fa-browser", Title = "لینک فوتر (گروه)")]
    [CustomeAuthorizeFilter]
    public class FooterGroupExteraLinkController : Controller
    {
        readonly IFooterGroupExteraLinkService FooterGroupExteraLinkService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public FooterGroupExteraLinkController
            (
                IFooterGroupExteraLinkService FooterGroupExteraLinkService,
                ISiteSettingService SiteSettingService
            )
        {
            this.FooterGroupExteraLinkService = FooterGroupExteraLinkService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "لینک فوتر (گروه)", Icon = "fa-link", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "لینک فوتر (گروه)";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "FooterGroupExteraLink", new { area = "WebMainAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست لینک فوتر (گروه)", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("WebMainAdmin", "FooterGroupExteraLink")));
        }

        [AreaConfig(Title = "افزودن لینک فوتر (گروه) جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] FooterGroupExteraLinkCreateUpdateVM input)
        {
            return Json(FooterGroupExteraLinkService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف لینک فوتر (گروه)", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(FooterGroupExteraLinkService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک لینک فوتر (گروه)", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(FooterGroupExteraLinkService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  لینک فوتر (گروه)", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] FooterGroupExteraLinkCreateUpdateVM input)
        {
            return Json(FooterGroupExteraLinkService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست لینک فوتر (گروه)", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] FooterGroupExteraLinkMainGrid searchInput)
        {
            return Json(FooterGroupExteraLinkService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] FooterGroupExteraLinkMainGrid searchInput)
        {
            var result = FooterGroupExteraLinkService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
