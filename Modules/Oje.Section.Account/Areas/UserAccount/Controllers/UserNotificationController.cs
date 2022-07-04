using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.AccountService.Models.View;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using System;

namespace Oje.Section.Account.Areas.UserAccount.Controllers
{
    [Area("UserAccount")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "حساب کاربری", Icon = "fa-users", Title = "پیام های ضروری")]
    [CustomeAuthorizeFilter]
    public class UserNotificationController : Controller
    {
        readonly IUserNotificationService UserNotificationService = null;
        readonly ISiteSettingService SiteSettingService = null;

        public UserNotificationController(
                IUserNotificationService UserNotificationService,
                ISiteSettingService SiteSettingService
            )
        {
            this.UserNotificationService = UserNotificationService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "پیام های ضروری", Icon = "fa-info", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "پیام های ضروری";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "UserNotification", new { area = "UserAccount" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست پیام های ضروری", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("UserAccount", "UserNotification")));
        }

        [AreaConfig(Title = "مشاهده پیام های ضروری", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetById([FromForm] string id)
        {
            return Json(UserNotificationService.GetBy(id, HttpContext.GetLoginUser()?.UserId, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست پیام های ضروری", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] UserNotificationMainGrid searchInput)
        {
            return Json(UserNotificationService.GetList(searchInput, HttpContext.GetLoginUser()?.UserId, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] UserNotificationMainGrid searchInput)
        {
            var result = UserNotificationService.GetList(searchInput, HttpContext.GetLoginUser()?.UserId, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
