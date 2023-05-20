using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.RegisterForm.Interfaces;
using Oje.Section.RegisterForm.Models.View;
using System;

namespace Oje.Section.RegisterForm.Areas.RegisterFormSupperAdmin.Controllers
{
    [Area("RegisterFormSupperAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "پایه ثبت نام کاربر", Icon = "fa-users", Title = "مبلغ فرم ثبت نام")]
    [CustomeAuthorizeFilter]
    public class UserRegisterFormPriceController : Controller
    {
        readonly IUserRegisterFormPriceService UserRegisterFormPriceService = null;
        readonly IUserRegisterFormService UserRegisterFormService = null;
        readonly ISiteSettingService SiteSettingService = null;

        public UserRegisterFormPriceController
            (
                IUserRegisterFormPriceService UserRegisterFormPriceService,
                IUserRegisterFormService UserRegisterFormService,
                ISiteSettingService SiteSettingService
            )
        {
            this.UserRegisterFormPriceService = UserRegisterFormPriceService;
            this.UserRegisterFormService = UserRegisterFormService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "مبلغ فرم ثبت نام", Icon = "fa-hand-holding-usd", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "مبلغ فرم ثبت نام";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "UserRegisterFormPrice", new { area = "RegisterFormSupperAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه مبلغ فرم ثبت نام", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("RegisterFormSupperAdmin", "UserRegisterFormPrice")));
        }

        [AreaConfig(Title = "افزودن مبلغ فرم ثبت نام جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] UserRegisterFormPriceCreateUpdateVM input)
        {
            return Json(UserRegisterFormPriceService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف مبلغ فرم ثبت نام", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(UserRegisterFormPriceService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک مبلغ فرم ثبت نام", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(UserRegisterFormPriceService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  مبلغ فرم ثبت نام", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] UserRegisterFormPriceCreateUpdateVM input)
        {
            return Json(UserRegisterFormPriceService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست مبلغ فرم ثبت نام", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] UserRegisterFormPriceMainGrid searchInput)
        {
            return Json(UserRegisterFormPriceService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] UserRegisterFormPriceMainGrid searchInput)
        {
            var result = UserRegisterFormPriceService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست فرم ثبت نام", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetFormList()
        {
            return Json(UserRegisterFormService.GetLightList(SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
