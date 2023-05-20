using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Secretariat.Interfaces;
using Oje.AccountService.Interfaces;
using Oje.Section.Secretariat.Models.View;
using System;


namespace Oje.Section.Secretariat.Areas.SecretariatSupperAdmin.Controllers
{
    [Area("SecretariatSupperAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات دبیر خانه", Icon = "fa-typewriter", Title = "تنظیمات سربرگ")]
    [CustomeAuthorizeFilter]
    public class SecretariatHeaderFooterController : Controller
    {
        readonly ISecretariatHeaderFooterService SecretariatHeaderFooterService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public SecretariatHeaderFooterController
            (
                ISecretariatHeaderFooterService SecretariatHeaderFooterService,
                ISiteSettingService SiteSettingService
            )
        {
            this.SecretariatHeaderFooterService = SecretariatHeaderFooterService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "تنظیمات سربرگ", Icon = "fa-palette", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "تنظیمات سربرگ";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "SecretariatHeaderFooter", new { area = "SecretariatSupperAdmin" });

            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه تنظیمات سربرگ", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("SecretariatSupperAdmin", "SecretariatHeaderFooter")));
        }

        [AreaConfig(Title = "افزودن تنظیمات سربرگ جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] SecretariatHeaderFooterCreateUpdateVM input)
        {
            return Json(SecretariatHeaderFooterService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف تنظیمات سربرگ", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(SecretariatHeaderFooterService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک تنظیمات سربرگ", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(SecretariatHeaderFooterService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  تنظیمات سربرگ", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] SecretariatHeaderFooterCreateUpdateVM input)
        {
            return Json(SecretariatHeaderFooterService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست تنظیمات سربرگ", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] SecretariatHeaderFooterMainGrid searchInput)
        {
            return Json(SecretariatHeaderFooterService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] SecretariatHeaderFooterMainGrid searchInput)
        {
            var result = SecretariatHeaderFooterService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
