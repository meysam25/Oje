using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.FileService.Interfaces;
using Oje.FileService.Models.View;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;

namespace Oje.Section.Security.Areas.Security.Controllers
{
    [Area("Security")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "امنیت", Icon = "fa-user-secret", Title = "کلیه فایل ها")]
    [CustomeAuthorizeFilter]
    public class UploadedFileController : Controller
    {
        readonly IUploadedFileService UploadedFileService = null;
        readonly ISiteSettingService SiteSettingService = null;

        public UploadedFileController
            (
                IUploadedFileService UploadedFileService,
                ISiteSettingService SiteSettingService
            )
        {
            this.UploadedFileService = UploadedFileService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "کلیه فایل ها", Icon = "fa-image", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "کلیه فایل ها";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "UploadedFile", new { area = "Security" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست کلیه فایل ها", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Security", "UploadedFile")));
        }

        [AreaConfig(Title = "حذف کلیه فایل ها", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(UploadedFileService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست کلیه فایل ها", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] UploadedFileMainGrid searchInput)
        {
            return Json(UploadedFileService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
