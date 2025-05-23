﻿using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.RegisterForm.Interfaces;
using Oje.Section.RegisterForm.Models.View;
using System;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.RegisterForm.Areas.RegisterFormSupperAdmin.Controllers
{
    [Area("RegisterFormSupperAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "پایه ثبت نام کاربر", Icon = "fa-users", Title = "فرم ثبت نام کاربر")]
    [CustomeAuthorizeFilter]
    public class UserRegisterFormController : Controller
    {
        readonly IUserRegisterFormService UserRegisterFormService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly Interfaces.IUserService UserService = null;
        readonly Interfaces.IRoleService RoleService = null;
        readonly IUserRegisterFormCategoryService UserRegisterFormCategoryService = null;
        public UserRegisterFormController
            (
                IUserRegisterFormService UserRegisterFormService,
                ISiteSettingService SiteSettingService,
                Interfaces.IUserService UserService,
                Interfaces.IRoleService RoleService,
                IUserRegisterFormCategoryService UserRegisterFormCategoryService
            )
        {
            this.UserRegisterFormService = UserRegisterFormService;
            this.SiteSettingService = SiteSettingService;
            this.UserService = UserService;
            this.RoleService = RoleService;
            this.UserRegisterFormCategoryService = UserRegisterFormCategoryService;
        }

        [AreaConfig(Title = "فرم ثبت نام کاربر", Icon = "fa-user", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "فرم ثبت نام کاربر";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "UserRegisterForm", new { area = "RegisterFormSupperAdmin" });

            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه فرم ثبت نام کاربر", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("RegisterFormSupperAdmin", "UserRegisterForm")));
        }

        [AreaConfig(Title = "افزودن فرم ثبت نام کاربر جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] UserRegisterFormCreateUpdateVM input)
        {
            return Json(UserRegisterFormService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف فرم ثبت نام کاربر", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(UserRegisterFormService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک فرم ثبت نام کاربر", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(UserRegisterFormService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  فرم ثبت نام کاربر", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] UserRegisterFormCreateUpdateVM input)
        {
            return Json(UserRegisterFormService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست فرم ثبت نام کاربر", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] UserRegisterFormMainGrid searchInput)
        {
            return Json(UserRegisterFormService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] UserRegisterFormMainGrid searchInput)
        {
            var result = UserRegisterFormService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
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

        [AreaConfig(Title = "مشاهده لیست نقش", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetRoleList()
        {
            return Json(RoleService.GetLightList(SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست گروه بندی", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCategories()
        {
            return Json(UserRegisterFormCategoryService.GetLightList(SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
