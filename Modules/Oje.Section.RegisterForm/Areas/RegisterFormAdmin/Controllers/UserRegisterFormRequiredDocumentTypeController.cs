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
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.RegisterForm.Areas.RegisterFormAdmin.Controllers
{
    [Area("RegisterFormAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "ثبت نام کاربر", Icon = "fa-users", Title = "نوع مدارک مورد نیاز فرم ثبت نام")]
    [CustomeAuthorizeFilter]
    public class UserRegisterFormRequiredDocumentTypeController: Controller
    {
        readonly IUserRegisterFormRequiredDocumentTypeService UserRegisterFormRequiredDocumentTypeService = null;
        readonly IUserRegisterFormService UserRegisterFormService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public UserRegisterFormRequiredDocumentTypeController
            (
                IUserRegisterFormRequiredDocumentTypeService UserRegisterFormRequiredDocumentTypeService,
                ISiteSettingService SiteSettingService,
                IUserRegisterFormService UserRegisterFormService
            )
        {
            this.UserRegisterFormRequiredDocumentTypeService = UserRegisterFormRequiredDocumentTypeService;
            this.SiteSettingService = SiteSettingService;
            this.UserRegisterFormService = UserRegisterFormService;
        }

        [AreaConfig(Title = "نوع مدارک مورد نیاز فرم ثبت نام", Icon = "fa-object-group", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "نوع مدارک مورد نیاز فرم ثبت نام";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "UserRegisterFormRequiredDocumentType", new { area = "RegisterFormAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه نوع مدارک مورد نیاز فرم ثبت نام", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("RegisterFormAdmin", "UserRegisterFormRequiredDocumentType")));
        }

        [AreaConfig(Title = "افزودن نوع مدارک مورد نیاز فرم ثبت نام جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] UserRegisterFormRequiredDocumentTypeCreateUpdateVM input)
        {
            return Json(UserRegisterFormRequiredDocumentTypeService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف نوع مدارک مورد نیاز فرم ثبت نام", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(UserRegisterFormRequiredDocumentTypeService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک نوع مدارک مورد نیاز فرم ثبت نام", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(UserRegisterFormRequiredDocumentTypeService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  نوع مدارک مورد نیاز فرم ثبت نام", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] UserRegisterFormRequiredDocumentTypeCreateUpdateVM input)
        {
            return Json(UserRegisterFormRequiredDocumentTypeService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست نوع مدارک مورد نیاز فرم ثبت نام", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] UserRegisterFormRequiredDocumentTypeMainGrid searchInput)
        {
            return Json(UserRegisterFormRequiredDocumentTypeService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] UserRegisterFormRequiredDocumentTypeMainGrid searchInput)
        {
            var result = UserRegisterFormRequiredDocumentTypeService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست فرم های ثبت نام", Icon = "fa-list-alt")]
        [HttpPost]
        public IActionResult GetFormList()
        {
            return Json(UserRegisterFormService.GetLightList(SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
