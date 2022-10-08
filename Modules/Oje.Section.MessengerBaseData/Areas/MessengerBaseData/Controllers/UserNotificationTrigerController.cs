using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.AccountService.Models.View;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using System;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.MessengerBaseData.Areas.MessengerBaseData.Controllers
{
    [Area("MessengerBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات پیام رسان", Icon = "fa-flag", Title = "تنظیمات ارسال نوتیفیکیشن")]
    [CustomeAuthorizeFilter]
    public class UserNotificationTrigerController : Controller
    {
        readonly IUserNotificationTrigerService UserNotificationTrigerService = null;
        readonly IRoleService RoleService = null;
        readonly IUserService UserService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public UserNotificationTrigerController(
                IUserNotificationTrigerService UserNotificationTrigerService,
                IRoleService RoleService,
                IUserService UserService,
                ISiteSettingService SiteSettingService
            )
        {
            this.UserNotificationTrigerService = UserNotificationTrigerService;
            this.RoleService = RoleService;
            this.UserService = UserService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "تنظیمات ارسال نوتیفیکیشن", Icon = "fa-comment", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "تنظیمات ارسال نوتیفیکیشن";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "UserNotificationTriger", new { area = "MessengerBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست تنظیمات ارسال نوتیفیکیشن", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("MessengerBaseData", "UserNotificationTriger")));
        }

        [AreaConfig(Title = "افزودن تنظیمات ارسال نوتیفیکیشن جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateUserNotificationTrigerVM input)
        {
            return Json(UserNotificationTrigerService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف تنظیمات ارسال نوتیفیکیشن", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(UserNotificationTrigerService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک تنظیمات ارسال نوتیفیکیشن", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(UserNotificationTrigerService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  تنظیمات ارسال نوتیفیکیشن", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateUserNotificationTrigerVM input)
        {
            return Json(UserNotificationTrigerService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست تنظیمات ارسال نوتیفیکیشن", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] UserNotificationTrigerMainGrid searchInput)
        {
            return Json(UserNotificationTrigerService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] UserNotificationTrigerMainGrid searchInput)
        {
            var result = UserNotificationTrigerService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }


        [AreaConfig(Title = "مشاهده لیست کاربران", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetUserList([FromQuery] Select2SearchVM searchInput, [FromQuery] int? cSOWSiteSettingId)
        {
            return Json(UserService.GetSelect2List(searchInput, HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست نقش ها", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetRoleList()
        {
            return Json(RoleService.GetRoleLightListForUser(UserService.GetLoginUser(), SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
