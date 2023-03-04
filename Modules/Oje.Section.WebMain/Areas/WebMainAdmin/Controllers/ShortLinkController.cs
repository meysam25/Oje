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

namespace Oje.Section.WebMain.Areas.WebMainAdmin.Controllers
{
    [Area("WebMainAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات وب سایت", Icon = "fa-browser", Title = "لینک کوتاه")]
    [CustomeAuthorizeFilter]
    public class ShortLinkController : Controller
    {
        readonly ISiteSettingService SiteSettingService = null;
        readonly IShortLinkService ShortLinkService = null;
        public ShortLinkController
            (
                ISiteSettingService SiteSettingService,
                IShortLinkService ShortLinkService
            )
        {
            this.SiteSettingService = SiteSettingService;
            this.ShortLinkService = ShortLinkService;
        }

        [AreaConfig(Title = "لینک کوتاه", Icon = "fa-link", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "لینک کوتاه";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ShortLink", new { area = "WebMainAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست لینک کوتاه", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("WebMainAdmin", "ShortLink")));
        }

        [AreaConfig(Title = "افزودن لینک کوتاه جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] ShortLinkCreateUpdateVM input)
        {
            return Json(ShortLinkService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف لینک کوتاه", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(ShortLinkService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک لینک کوتاه", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalLongId input)
        {
            return Json(ShortLinkService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  لینک کوتاه", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] ShortLinkCreateUpdateVM input)
        {
            return Json(ShortLinkService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست لینک کوتاه", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] ShortLinkMainGrid searchInput)
        {
            return Json(ShortLinkService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ShortLinkMainGrid searchInput)
        {
            var result = ShortLinkService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
