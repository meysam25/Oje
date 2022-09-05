using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Services;
using Oje.Security.Models.View;
using System;
using Oje.Security.Interfaces;

namespace Oje.Section.Security.Areas.Security.Controllers
{
    [Area("Security")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "امنیت", Icon = "fa-user-secret", Title = "لاگ لاگ نسخه های پشتیبانی")]
    [CustomeAuthorizeFilter]
    public class GoogleBackupArchiveLogController: Controller
    {
        readonly IGoogleBackupArchiveLogService GoogleBackupArchiveLogService = null;

        public GoogleBackupArchiveLogController
            (
                IGoogleBackupArchiveLogService GoogleBackupArchiveLogService
            )
        {
            this.GoogleBackupArchiveLogService = GoogleBackupArchiveLogService;
        }

        [AreaConfig(Title = "لاگ نسخه های پشتیبانی", Icon = "fa-warehouse", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "لاگ نسخه های پشتیبانی";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "GoogleBackupArchiveLog", new { area = "Security" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست لاگ نسخه های پشتیبانی", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Security", "GoogleBackupArchiveLog")));
        }

        [AreaConfig(Title = "مشاهده لیست لاگ نسخه های پشتیبانی", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] GoogleBackupArchiveLogMainGrid searchInput)
        {
            return Json(GoogleBackupArchiveLogService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] GoogleBackupArchiveLogMainGrid searchInput)
        {
            var result = GoogleBackupArchiveLogService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
