using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Services;
using Oje.Security.Interfaces;
using Oje.Security.Models.View;
using System;

namespace Oje.Section.Security.Areas.Security.Controllers
{
    [Area("Security")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "امنیت", Icon = "fa-user-secret", Title = "نسخه های پشتیبانی")]
    [CustomeAuthorizeFilter]
    public class GoogleBackupArchiveController: Controller
    {
        readonly IGoogleBackupArchiveService GoogleBackupArchiveService = null;

        public GoogleBackupArchiveController
            (
                IGoogleBackupArchiveService GoogleBackupArchiveService
            )
        {
            this.GoogleBackupArchiveService = GoogleBackupArchiveService;
        }

        [AreaConfig(Title = "نسخه های پشتیبانی", Icon = "fa-file-archive", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "نسخه های پشتیبانی";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "GoogleBackupArchive", new { area = "Security" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست نسخه های پشتیبانی", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Security", "GoogleBackupArchive")));
        }

        [AreaConfig(Title = "مشاهده لیست نسخه های پشتیبانی", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] GoogleBackupArchiveMainGrid searchInput)
        {
            return Json(GoogleBackupArchiveService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] GoogleBackupArchiveMainGrid searchInput)
        {
            var result = GoogleBackupArchiveService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
