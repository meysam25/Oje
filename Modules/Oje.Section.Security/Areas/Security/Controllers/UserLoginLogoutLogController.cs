using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Services;
using Oje.Security.Interfaces;
using Oje.Security.Models.View;
using System;

namespace Oje.Section.Security.Areas.Security.Controllers
{
    [Area("Security")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "امنیت", Icon = "fa-user-secret", Title = "لاگ ورود/خروج کاربر")]
    [CustomeAuthorizeFilter]
    public class UserLoginLogoutLogController: Controller
    {
        readonly IUserLoginLogoutLogService UserLoginLogoutLogService = null;
        readonly ISiteSettingService SiteSettingService = null;

        public UserLoginLogoutLogController
            (
                IUserLoginLogoutLogService UserLoginLogoutLogService,
                ISiteSettingService SiteSettingService
            )
        {
            this.UserLoginLogoutLogService = UserLoginLogoutLogService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "لاگ ورود/خروج کاربر", Icon = "fa-clipboard-check", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "لاگ ورود/خروج کاربر";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "UserLoginLogoutLog", new { area = "Security" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست لاگ ورود/خروج کاربر", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Security", "UserLoginLogoutLog")));
        }

        [AreaConfig(Title = "مشاهده لیست لاگ ورود/خروج کاربر", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] UserLoginLogoutLogMainGrid searchInput)
        {
            return Json(UserLoginLogoutLogService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] UserLoginLogoutLogMainGrid searchInput)
        {
            var result = UserLoginLogoutLogService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
