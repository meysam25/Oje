using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.RegisterForm.Models.View;
using System;
using Oje.Section.RegisterForm.Interfaces;
using Oje.AccountService.Interfaces;

namespace Oje.Section.RegisterForm.Areas.RegisterFormAdmin.Controllers
{
    [Area("RegisterFormAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "ثبت نام کاربر", Icon = "fa-users", Title = "گروه بندی فرم")]
    [CustomeAuthorizeFilter]
    public class UserRegisterFormCategoryController: Controller
    {
        readonly IUserRegisterFormCategoryService UserRegisterFormCategoryService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public UserRegisterFormCategoryController
            (
                IUserRegisterFormCategoryService UserRegisterFormCategoryService,
                ISiteSettingService SiteSettingService
            )
        {
            this.UserRegisterFormCategoryService = UserRegisterFormCategoryService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "گروه بندی فرم", Icon = "fa-object-group", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "گروه بندی فرم";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "UserRegisterFormCategory", new { area = "RegisterFormAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه گروه بندی فرم", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("RegisterFormAdmin", "UserRegisterFormCategory")));
        }

        [AreaConfig(Title = "افزودن گروه بندی فرم جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] UserRegisterFormCategoryCreateUpdateVM input)
        {
            return Json(UserRegisterFormCategoryService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف گروه بندی فرم", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(UserRegisterFormCategoryService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک گروه بندی فرم", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(UserRegisterFormCategoryService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  گروه بندی فرم", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] UserRegisterFormCategoryCreateUpdateVM input)
        {
            return Json(UserRegisterFormCategoryService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست گروه بندی فرم", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] UserRegisterFormCategoryMainGrid searchInput)
        {
            return Json(UserRegisterFormCategoryService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] UserRegisterFormCategoryMainGrid searchInput)
        {
            var result = UserRegisterFormCategoryService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
