using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.FileService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Services;
using Oje.Section.WebMain.Hubs.Models;
using Oje.Section.WebMain.Models.View;

namespace Oje.Section.WebMain.Areas.Controllers
{
    [Area("WebMainAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "وب سایت", Icon = "fa-browser", Title = "پشتیبانی آنلاین")]
    [CustomeAuthorizeFilter]
    public class OnlineSupportController: Controller
    {
        readonly ISiteSettingService SiteSettingService = null;
        readonly IUploadedFileService UploadedFileService = null;
        public OnlineSupportController
            (
                ISiteSettingService SiteSettingService, 
                IUploadedFileService UploadedFileService
            )
        {
            this.SiteSettingService = SiteSettingService;
            this.UploadedFileService = UploadedFileService;
        }

        [AreaConfig(Title = "پشتیبانی آنلاین", Icon = "fa-headset", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "پشتیبانی آنلاین";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "OnlineSupport", new { area = "WebMainAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست پشتیبانی آنلاین", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("WebMainAdmin", "OnlineSupport")));
        }

        [AreaConfig(Title = "حذف پشتیبانی آنلاین", Icon = "fa-trash-o")]
        [HttpPost]
        public ActionResult Delete(long id)
        {
            return Json(SupportUserInfo.Delete(id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست پاسخ های خودکار", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] OnlineSupportMainGrid searchInput)
        {
            return Json(SupportUserInfo.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "افزودن فایل جدید", Icon = "fa-plus")]
        [HttpPost]
        public ActionResult UploadNewFileForOnlineChat([FromForm] IFormFile mainFile, [FromForm] long? userId)
        {
            if (userId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            var tempResult = UploadedFileService.UploadNewFile(FileType.OnlineFile, mainFile, userId, SiteSettingService.GetSiteSetting()?.Id, null, ".jpg,.jpeg,.png,.doc,.pdf", true);
            if (!string.IsNullOrEmpty(tempResult))
                tempResult = GlobalConfig.FileAccessHandlerUrl + tempResult;
            return Json(tempResult);
        }

        [AreaConfig(Title = "افزودن ویس جدید", Icon = "fa-plus")]
        [HttpPost]
        public ActionResult UploadNewVoiceForOnlineChat([FromForm] IFormFile mainFile, [FromForm] long? userId)
        {
            if (userId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            var tempResult = UploadedFileService.UploadNewFile(FileType.OnlineFile, mainFile, userId, SiteSettingService.GetSiteSetting()?.Id, null, ".ogg,.webm", true);
            if (!string.IsNullOrEmpty(tempResult))
                tempResult = GlobalConfig.FileAccessHandlerUrl + tempResult;
            return Json(tempResult);
        }
    }
}
