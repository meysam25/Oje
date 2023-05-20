using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Secretariat.Interfaces;
using Oje.Section.Secretariat.Models.View;
using Oje.AccountService.Interfaces;
using System;

namespace Oje.Section.Secretariat.Areas.Secretariat.Controllers
{
    [Area("Secretariat")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "دبیر خانه", Icon = "fa-typewriter", Title = "نامه های من")]
    [CustomeAuthorizeFilter]
    public class MySecretariatLetterController: Controller
    {
        readonly ISecretariatLetterService SecretariatLetterService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly ISecretariatLetterCategoryService SecretariatLetterCategoryService = null;
        readonly ISecretariatUserDigitalSignatureService SecretariatUserDigitalSignatureService = null;
        readonly ISecretariatLetterUserService SecretariatLetterUserService = null;

        public MySecretariatLetterController
           (
               ISecretariatLetterService SecretariatLetterService,
               ISiteSettingService SiteSettingService,
               ISecretariatLetterCategoryService SecretariatLetterCategoryService,
               ISecretariatUserDigitalSignatureService SecretariatUserDigitalSignatureService,
               ISecretariatLetterUserService SecretariatLetterUserService
           )
        {
            this.SecretariatLetterService = SecretariatLetterService;
            this.SiteSettingService = SiteSettingService;
            this.SecretariatLetterCategoryService = SecretariatLetterCategoryService;
            this.SecretariatUserDigitalSignatureService = SecretariatUserDigitalSignatureService;
            this.SecretariatLetterUserService = SecretariatLetterUserService;
        }

        [AreaConfig(Title = "نامه های من", Icon = "fa-envelope", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "نامه های من";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "MySecretariatLetter", new { area = "Secretariat" });

            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست نامه های من", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Secretariat", "MySecretariatLetter")));
        }

        [AreaConfig(Title = "مشاهده لیست نامه های من", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] SecretariatLetterMainGrid searchInput)
        {
            return Json(SecretariatLetterService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] SecretariatLetterMainGrid searchInput)
        {
            var result = SecretariatLetterService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "دانلود پی دی اف نامه", Icon = "fa-download")]
        [HttpGet]
        public IActionResult DownloadPdf([FromQuery] GlobalLongId input)
        {
            return File(
                    HtmlToPdfBlink.Convert((Request.IsHttps ? "https" : "http") + "://" + Request.Host + Url.Action("Detailes", "MySecretariatLetter", new { area = "Secretariat", input.id, isPrint = true }), Request.Cookies),
                    System.Net.Mime.MediaTypeNames.Application.Pdf, DateTime.Now.ToFaDate("_") + "_" + DateTime.Now.ToString("HH_mm_ss") + ".pdf"
                );
        }

        [AreaConfig(Title = "جزییات", Icon = "fa-eye")]
        [HttpGet]
        public IActionResult Detailes([FromQuery] long id, [FromQuery] bool isPrint = false)
        {
            ViewBag.isPrint = isPrint;
            var foundItem = SecretariatLetterService.PdfDetailes(id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId);
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            ViewBag.baseUrl = (Request.IsHttps ? "https" : "http") + "://" + Request.Host;

            return View(foundItem);
        }

        [AreaConfig(Title = "مشاهده لیست کاربران مجاز", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetValidUsers([FromForm] SecretariatLetterUserMainGrid searchInput)
        {
            return Json(SecretariatLetterUserService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId));
        }

        [AreaConfig(Title = "افزودن کاربر ارجاع", Icon = "fa-plus")]
        [HttpPost]
        public ActionResult CreateValidUsers([FromForm] SecretariatLetterUserCreateVM input)
        {
            return Json(SecretariatLetterUserService.CreateForWeb(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId));
        }

        [AreaConfig(Title = "حذف کاربر", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult DeleteUser([FromForm] GlobalLongId input, [FromForm] long? pKey)
        {
            return Json(SecretariatLetterUserService.Delete(pKey, input?.id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId));
        }

        [AreaConfig(Title = "مشاهده لیست فایل ضمیمه", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetFileList([FromForm] GlobalGridParentLong input)
        {
            return Json(SecretariatLetterService.GetUploadFiles(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId));
        }
    }
}
