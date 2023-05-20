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
using Microsoft.AspNetCore.Http;
using System;

namespace Oje.Section.Secretariat.Areas.Secretariat.Controllers
{
    [Area("Secretariat")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "دبیر خانه", Icon = "fa-typewriter", Title = "نامه")]
    [CustomeAuthorizeFilter]
    public class SecretariatLetterController: Controller
    {
        readonly ISecretariatLetterService SecretariatLetterService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly ISecretariatLetterCategoryService SecretariatLetterCategoryService = null;
        readonly ISecretariatUserDigitalSignatureService SecretariatUserDigitalSignatureService = null;
        readonly ISecretariatLetterUserService SecretariatLetterUserService = null;

        public SecretariatLetterController
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

        [AreaConfig(Title = "نامه", Icon = "fa-envelope", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "نامه";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "SecretariatLetter", new { area = "Secretariat" });

            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست نامه", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Secretariat", "SecretariatLetter")));
        }

        [AreaConfig(Title = "افزودن نامه جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] SecretariatLetterCreateUpdateVM input)
        {
            return Json(SecretariatLetterService.Create(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId));
        }

        [AreaConfig(Title = "حذف نامه", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(SecretariatLetterService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک نامه", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalLongId input)
        {
            return Json(SecretariatLetterService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  نامه", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] SecretariatLetterCreateUpdateVM input)
        {
            return Json(SecretariatLetterService.Update(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId));
        }

        [AreaConfig(Title = "مشاهده لیست نامه", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] SecretariatLetterMainGrid searchInput)
        {
            return Json(SecretariatLetterService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] SecretariatLetterMainGrid searchInput)
        {
            var result = SecretariatLetterService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
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
                    HtmlToPdfBlink.Convert((Request.IsHttps ? "https" : "http") + "://" + Request.Host + Url.Action("Detailes", "SecretariatLetter", new { area = "Secretariat", input.id, isPrint = true }), Request.Cookies),
                    System.Net.Mime.MediaTypeNames.Application.Pdf, DateTime.Now.ToFaDate("_") + "_" + DateTime.Now.ToString("HH_mm_ss") + ".pdf"
                );
        }

        [AreaConfig(Title = "جزییات", Icon = "fa-eye")]
        [HttpGet]
        public IActionResult Detailes([FromQuery] long id, [FromQuery] bool isPrint = false)
        {
            ViewBag.isPrint = isPrint;
            var foundItem = SecretariatLetterService.PdfDetailes(id, SiteSettingService.GetSiteSetting()?.Id, null);
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            ViewBag.baseUrl = (Request.IsHttps ? "https" : "http") + "://" + Request.Host;

            return View(foundItem);
        }

        [AreaConfig(Title = "تایید نامه", Icon = "fa-pencil")]
        [HttpPost]
        public ActionResult Confirm([FromForm] GlobalLongId input)
        {
            return Json(SecretariatLetterService.Confirm(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست کاربران مجاز", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetValidUsers([FromForm] SecretariatLetterUserMainGrid searchInput)
        {
            return Json(SecretariatLetterUserService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, null));
        }

        [AreaConfig(Title = "مشاهده لیست فایل ضمیمه", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetFileList([FromForm] GlobalGridParentLong input)
        {
            return Json(SecretariatLetterService.GetUploadFiles(input, SiteSettingService.GetSiteSetting()?.Id, null));
        }

        [AreaConfig(Title = "حذف فایل ضمیمه", Icon = "fa-trash-o")]
        [HttpPost]
        public ActionResult DeleteFile([FromForm] GlobalLongId input, [FromForm] long? pKey)
        {
            return Json(SecretariatLetterService.DeleteUploadFile(input?.id, pKey, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "افزودن فایل ضمیمه", Icon = "fa-plus")]
        [HttpPost]
        public ActionResult UploadNewFile([FromForm] long? pKey, [FromForm] IFormFile mainFile, [FromForm] string title)
        {
            return Json(SecretariatLetterService.UploadNewFile(pKey, mainFile, title, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId));
        }


        [AreaConfig(Title = "مشاهده لیست گروه بندی", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCateogry()
        {
            return Json(SecretariatLetterCategoryService.GetLightList(SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست امضاکننده", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetSigner()
        {
            return Json(SecretariatUserDigitalSignatureService.GetLightList(SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
