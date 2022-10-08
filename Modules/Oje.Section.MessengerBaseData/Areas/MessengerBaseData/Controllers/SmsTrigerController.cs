using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Sms.Interfaces;
using Oje.Sms.Models.View;
using System;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.MessengerBaseData.Areas.MessengerBaseData.Controllers
{
    [Area("MessengerBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات پیام رسان", Icon = "fa-flag", Title = "تنظیمات ارسال پیامک")]
    [CustomeAuthorizeFilter]
    public class SmsTrigerController : Controller
    {
        readonly ISmsTrigerService SmsTrigerService = null;
        readonly AccountService.Interfaces.IRoleService RoleService = null;
        readonly AccountService.Interfaces.IUserService UserService = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;
        public SmsTrigerController(
                ISmsTrigerService SmsTrigerService,
                AccountService.Interfaces.IRoleService RoleService,
                AccountService.Interfaces.IUserService UserService,
                AccountService.Interfaces.ISiteSettingService SiteSettingService
            )
        {
            this.SmsTrigerService = SmsTrigerService;
            this.RoleService = RoleService;
            this.UserService = UserService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "تنظیمات ارسال پیامک", Icon = "fa-comment", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "تنظیمات ارسال پیامک";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "SmsTriger", new { area = "MessengerBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست تنظیمات ارسال پیامک", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("MessengerBaseData", "SmsTriger")));
        }

        [AreaConfig(Title = "افزودن تنظیمات ارسال پیامک جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateSmsTrigerVM input)
        {
            return Json(SmsTrigerService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف تنظیمات ارسال پیامک", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(SmsTrigerService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک تنظیمات ارسال پیامک", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(SmsTrigerService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  تنظیمات ارسال پیامک", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateSmsTrigerVM input)
        {
            return Json(SmsTrigerService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست تنظیمات ارسال پیامک", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] SmsTrigerMainGrid searchInput)
        {
            return Json(SmsTrigerService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] SmsTrigerMainGrid searchInput)
        {
            var result = SmsTrigerService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }


        [AreaConfig(Title = "مشاهده لیست کاربران", Icon = "fa-list-alt ")]
        [HttpGet]
        public ActionResult GetUserList([FromQuery] Select2SearchVM searchInput, [FromQuery] int? cSOWSiteSettingId)
        {
            return Json(UserService.GetSelect2List(searchInput, HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست نقش ها", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetRoleList()
        {
            return Json(RoleService.GetRoleLightListForUser(UserService.GetLoginUser(), SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
