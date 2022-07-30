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

namespace Oje.Section.RegisterForm.Areas.RegisterFormAdmin.Controllers
{
    [Area("RegisterFormAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "ثبت نام کاربر", Icon = "fa-users", Title = "کد تخفیف (کمپین)")]
    [CustomeAuthorizeFilter]
    public class UserRegisterFormDiscountCodeController: Controller
    {
        readonly IUserRegisterFormDiscountCodeService UserRegisterFormDiscountCodeService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IUserRegisterFormService UserRegisterFormService = null;

        public UserRegisterFormDiscountCodeController
            (
                IUserRegisterFormDiscountCodeService UserRegisterFormDiscountCodeService,
                ISiteSettingService SiteSettingService,
                IUserRegisterFormService UserRegisterFormService
            )
        {
            this.UserRegisterFormDiscountCodeService = UserRegisterFormDiscountCodeService;
            this.SiteSettingService = SiteSettingService;
            this.UserRegisterFormService = UserRegisterFormService;
        }

        [AreaConfig(Title = "کد تخفیف (کمپین)", Icon = "fa-percent", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "کد تخفیف (کمپین)";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "UserRegisterFormDiscountCode", new { area = "RegisterFormAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه کد تخفیف (کمپین)", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("RegisterFormAdmin", "UserRegisterFormDiscountCode")));
        }

        [AreaConfig(Title = "افزودن کد تخفیف (کمپین) جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] UserRegisterFormDiscountCodeCreateUpdateVM input)
        {
            return Json(UserRegisterFormDiscountCodeService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده یک کد تخفیف (کمپین)", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalLongId input)
        {
            return Json(UserRegisterFormDiscountCodeService.GetBy(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف کد تخفیف (کمپین)", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(UserRegisterFormDiscountCodeService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی کد تخفیف (کمپین) نام", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] UserRegisterFormDiscountCodeCreateUpdateVM input)
        {
            return Json(UserRegisterFormDiscountCodeService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }


        [AreaConfig(Title = "مشاهده لیست کد تخفیف (کمپین)", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] UserRegisterFormDiscountCodeMainGrid searchInput)
        {
            return Json(UserRegisterFormDiscountCodeService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] UserRegisterFormDiscountCodeMainGrid searchInput)
        {
            var result = UserRegisterFormDiscountCodeService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }


        [AreaConfig(Title = "مشاهده لیست فرم ثبت نام", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetProposalFormList()
        {
            return Json(UserRegisterFormService.GetLightList(SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
