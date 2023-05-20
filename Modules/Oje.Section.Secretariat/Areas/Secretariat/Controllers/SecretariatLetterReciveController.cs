using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Secretariat.Interfaces;
using Oje.Section.Secretariat.Models.View;
using System;
using Microsoft.AspNetCore.Http;
using Oje.Section.Secretariat.Services;

namespace Oje.Section.Secretariat.Areas.Secretariat.Controllers
{
    [Area("Secretariat")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "دبیر خانه", Icon = "fa-typewriter", Title = "نامه های دریافتی")]
    [CustomeAuthorizeFilter]
    public class SecretariatLetterReciveController: Controller
    {
        readonly ISecretariatLetterReciveService SecretariatLetterReciveService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public SecretariatLetterReciveController
            (
                ISecretariatLetterReciveService SecretariatLetterReciveService,
                ISiteSettingService SiteSettingService
            )
        {
            this.SecretariatLetterReciveService = SecretariatLetterReciveService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "نامه های دریافتی", Icon = "fa-envelope", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "نامه های دریافتی";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "SecretariatLetterRecive", new { area = "Secretariat" });

            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست نامه های دریافتی", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Secretariat", "SecretariatLetterRecive")));
        }

        [AreaConfig(Title = "افزودن نامه های دریافتی جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] SecretariatLetterReciveCreateUpdateVM input)
        {
            return Json(SecretariatLetterReciveService.Create(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId));
        }

        [AreaConfig(Title = "حذف نامه های دریافتی", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(SecretariatLetterReciveService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک نامه های دریافتی", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalLongId input)
        {
            return Json(SecretariatLetterReciveService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  نامه های دریافتی", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] SecretariatLetterReciveCreateUpdateVM input)
        {
            return Json(SecretariatLetterReciveService.Update(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId));
        }

        [AreaConfig(Title = "مشاهده لیست نامه های دریافتی", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] SecretariatLetterReciveMainGrid searchInput)
        {
            return Json(SecretariatLetterReciveService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] SecretariatLetterReciveMainGrid searchInput)
        {
            var result = SecretariatLetterReciveService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }


        [AreaConfig(Title = "مشاهده لیست فایل ضمیمه", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetFileList([FromForm] GlobalGridParentLong input)
        {
            return Json(SecretariatLetterReciveService.GetUploadFiles(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف فایل ضمیمه", Icon = "fa-trash-o")]
        [HttpPost]
        public ActionResult DeleteFile([FromForm] GlobalLongId input, [FromForm] long? pKey)
        {
            return Json(SecretariatLetterReciveService.DeleteUploadFile(input?.id, pKey, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "افزودن فایل ضمیمه", Icon = "fa-plus")]
        [HttpPost]
        public ActionResult UploadNewFile([FromForm] long? pKey, [FromForm] IFormFile mainFile, [FromForm] string title)
        {
            return Json(SecretariatLetterReciveService.UploadNewFile(pKey, mainFile, title, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId));
        }
    }
}
