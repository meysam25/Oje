using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Services;
using System;
using Oje.ValidatedSignature.Interfaces;
using Oje.ValidatedSignature.Models.View;

namespace Oje.Section.ValidatedSignature.Areas.Signature.Controllers
{
    [Area("Signature")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "اعتبارسنجی امضا", Icon = "fa-spider-black-widow", Title = "تنظیمات وب سایت")]
    [CustomeAuthorizeFilter]
    public class SiteSettingController: Controller
    {
        readonly ISiteSettingService SiteSettingService = null;

        public SiteSettingController
            (
                ISiteSettingService SiteSettingService
            )
        {
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "تنظیمات وب سایت", Icon = "fa-check-circle", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "تنظیمات وب سایت";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "SiteSetting", new { area = "Signature" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست تنظیمات وب سایت", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Signature", "SiteSetting")));
        }

        [AreaConfig(Title = "مشاهده جزئیات تغییرات غیر معتبر", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] long? id)
        {
            return Json(SiteSettingService.GetBy(id));
        }

        [AreaConfig(Title = "مشاهده لیست تنظیمات وب سایت", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] SiteSettingMainGrid searchInput)
        {
            return Json(SiteSettingService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] SiteSettingMainGrid searchInput)
        {
            var result = SiteSettingService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
