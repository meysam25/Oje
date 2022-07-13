using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.RegisterForm.Interfaces;
using Oje.Section.RegisterForm.Models.View;
using System;
using System.Collections.Generic;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.RegisterForm.Areas.RegisterFormAdmin.Controllers
{
    [Area("RegisterFormAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "ثبت نام کاربر", Icon = "fa-users", Title = "کاربران ثبت نام کرده پرداخت نشده")]
    [CustomeAuthorizeFilter]
    public class UserFilledRegisterFormNotPayedController: Controller
    {
        readonly IUserFilledRegisterFormService UserFilledRegisterFormService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly Interfaces.IRoleService RoleService = null;
        readonly IUserRegisterFormService UserRegisterFormService = null;

        static bool isPayed = false;

        public UserFilledRegisterFormNotPayedController
            (
                IUserFilledRegisterFormService UserFilledRegisterFormService,
                ISiteSettingService SiteSettingService,
                Interfaces.IRoleService RoleService,
                 IUserRegisterFormService UserRegisterFormService
            )
        {
            this.UserFilledRegisterFormService = UserFilledRegisterFormService;
            this.SiteSettingService = SiteSettingService;
            this.RoleService = RoleService;
            this.UserRegisterFormService = UserRegisterFormService;
        }

        [AreaConfig(Title = "کاربران ثبت نام کرده پرداخت نشده", Icon = "fa-user", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "کاربران ثبت نام کرده پرداخت نشده";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "UserFilledRegisterFormNotPayed", new { area = "RegisterFormAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه کاربران ثبت نام کرده پرداخت نشده", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("RegisterFormAdmin", "UserFilledRegisterFormNotPayed")));
        }

        [AreaConfig(Title = "افزودن فرم ثبت نام کاربر جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult CreateUser([FromForm] long? pKey, [FromForm] List<int> roleIds)
        {
            return Json(UserFilledRegisterFormService.CreateNewUser(pKey, SiteSettingService.GetSiteSetting()?.Id, SiteSettingService.GetSiteSetting()?.UserId, roleIds, HttpContext.GetLoginUser()?.UserId, isPayed));
        }

        [AreaConfig(Title = "مشاهده اسناد کاربران ثبت نام کرده پرداخت نشده", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetPPFImageList([FromForm] GlobalGridParentLong input)
        {
            return Json(UserFilledRegisterFormService.GetUploadImages(input, SiteSettingService.GetSiteSetting()?.Id, isPayed));
        }

        [AreaConfig(Title = "مشاهده یک کاربران ثبت نام کرده پرداخت نشده", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalLongId input, [FromQuery] bool? ignoreMaster)
        {
            ViewBag.targetLayout = "~/Areas/Account/Views/Shared/_LayoutAdmin.cshtml";
            ViewBag.ignoreMaster = ignoreMaster;
            return View("~/Views/Register/Details.cshtml", UserFilledRegisterFormService.PdfDetailes(input?.id, SiteSettingService.GetSiteSetting()?.Id, null, false, isPayed));
        }

        [AreaConfig(Title = "حذف کاربران ثبت نام کرده پرداخت نشده", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(UserFilledRegisterFormService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id, isPayed));
        }

        [AreaConfig(Title = "مشاهده لیست کاربران ثبت نام کرده پرداخت نشده", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] UserFilledRegisterFormMainGrid searchInput)
        {
            return Json(UserFilledRegisterFormService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, isPayed));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] UserFilledRegisterFormMainGrid searchInput)
        {
            var result = UserFilledRegisterFormService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, isPayed);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست نقش ها", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetRoleList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(RoleService.GetList(SiteSettingService.GetSiteSetting()?.Id, searchInput));
        }

        [AreaConfig(Title = "مشاهده لیست فرم ها", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetFormList()
        {
            return Json(UserRegisterFormService.GetLightList2(SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
