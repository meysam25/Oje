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
    [AreaConfig(ModualTitle = "تنظیمات وب سایت", Icon = "fa-browser", Title = "معرفی صفحه")]
    [CustomeAuthorizeFilter]
    public class PageManifestController : Controller
    {
        readonly IPageManifestService PageManifestService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IPageService PageService = null;

        public PageManifestController
            (
                IPageManifestService PageManifestService,
                ISiteSettingService SiteSettingService,
                IPageService PageService
            )
        {
            this.PageManifestService = PageManifestService;
            this.SiteSettingService = SiteSettingService;
            this.PageService = PageService;
        }

        [AreaConfig(Title = "معرفی صفحه", Icon = "fa-clipboard-list", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "معرفی صفحه";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "PageManifest", new { area = "WebMainAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست معرفی صفحه", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("WebMainAdmin", "PageManifest")));
        }

        [AreaConfig(Title = "افزودن معرفی صفحه جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] PageManifestCreateUpdateVM input)
        {
            return Json(PageManifestService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف معرفی صفحه", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(PageManifestService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک معرفی صفحه", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(PageManifestService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  معرفی صفحه", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] PageManifestCreateUpdateVM input)
        {
            return Json(PageManifestService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست معرفی صفحه", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] PageManifestMainGrid searchInput)
        {
            return Json(PageManifestService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] PageManifestMainGrid searchInput)
        {
            var result = PageManifestService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست صفحه ها", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetPageLightList([FromForm] Select2SearchVM searchInput, [FromQuery] int? cSOWSiteSettingId)
        {
            return Json(PageService.GetSelect2(searchInput, HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
