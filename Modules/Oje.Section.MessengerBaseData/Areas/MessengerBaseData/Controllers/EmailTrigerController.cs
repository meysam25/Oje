using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.EmailService.Interfaces;
using Oje.EmailService.Models.View;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using System;

namespace Oje.Section.MessengerBaseData.Areas.MessengerBaseData.Controllers
{
    [Area("MessengerBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات پیام رسان", Icon = "fa-flag", Title = "تنظیمات ارسال ایمیل")]
    [CustomeAuthorizeFilter]
    public class EmailTrigerController: Controller
    {
        readonly IEmailTrigerService EmailTrigerService = null;
        readonly AccountService.Interfaces.IRoleService RoleService = null;
        readonly AccountService.Interfaces.IUserService UserService = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;
        public EmailTrigerController(
                IEmailTrigerService EmailTrigerService,
                AccountService.Interfaces.IRoleService RoleService,
                AccountService.Interfaces.IUserService UserService,
                AccountService.Interfaces.ISiteSettingService SiteSettingService
            )
        {
            this.EmailTrigerService = EmailTrigerService;
            this.RoleService = RoleService;
            this.UserService = UserService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "تنظیمات ارسال ایمیل", Icon = "fa-envelope", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "تنظیمات ارسال ایمیل";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "EmailTriger", new { area = "MessengerBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست تنظیمات ارسال ایمیل", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("MessengerBaseData", "EmailTriger")));
        }

        [AreaConfig(Title = "افزودن تنظیمات ارسال ایمیل جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] EmailTrigerCreateUpdateVM input)
        {
            return Json(EmailTrigerService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف تنظیمات ارسال ایمیل", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(EmailTrigerService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک تنظیمات ارسال ایمیل", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(EmailTrigerService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  تنظیمات ارسال ایمیل", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] EmailTrigerCreateUpdateVM input)
        {
            return Json(EmailTrigerService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست تنظیمات ارسال ایمیل", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] EmailTrigerMainGrid searchInput)
        {
            return Json(EmailTrigerService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] EmailTrigerMainGrid searchInput)
        {
            var result = EmailTrigerService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }


        [AreaConfig(Title = "مشاهده لیست کاربران", Icon = "fa-list-alt ")]
        [HttpGet]
        public ActionResult GetUserList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(UserService.GetSelect2List(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست نقش ها", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetRoleList()
        {
            return Json(RoleService.GetRoleLightListForUser(UserService.GetLoginUser(), SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
