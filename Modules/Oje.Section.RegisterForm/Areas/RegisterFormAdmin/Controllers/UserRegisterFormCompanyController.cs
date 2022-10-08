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
    [AreaConfig(ModualTitle = "ثبت نام کاربر", Icon = "fa-users", Title = "شرکت های ثبت نام")]
    [CustomeAuthorizeFilter]
    public class UserRegisterFormCompanyController: Controller
    {
        readonly IUserRegisterFormCompanyService UserRegisterFormCompanyService = null;
        readonly IUserRegisterFormService UserRegisterFormService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly Interfaces.ICompanyService CompanyService = null;

        public UserRegisterFormCompanyController
            (
                IUserRegisterFormCompanyService UserRegisterFormCompanyService,
                ISiteSettingService SiteSettingService,
                IUserRegisterFormService UserRegisterFormService,
                Interfaces.ICompanyService CompanyService
            )
        {
            this.UserRegisterFormCompanyService = UserRegisterFormCompanyService;
            this.SiteSettingService = SiteSettingService;
            this.UserRegisterFormService = UserRegisterFormService;
            this.CompanyService = CompanyService;
        }

        [AreaConfig(Title = "شرکت های ثبت نام", Icon = "fa-building", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "شرکت های ثبت نام";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "UserRegisterFormCompany", new { area = "RegisterFormAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه شرکت های ثبت نام", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("RegisterFormAdmin", "UserRegisterFormCompany")));
        }

        [AreaConfig(Title = "افزودن شرکت های ثبت نام جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] UserRegisterFormCompanyCreateUpdateVM input)
        {
            return Json(UserRegisterFormCompanyService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف شرکت های ثبت نام", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(UserRegisterFormCompanyService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک شرکت های ثبت نام", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(UserRegisterFormCompanyService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  شرکت های ثبت نام", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] UserRegisterFormCompanyCreateUpdateVM input)
        {
            return Json(UserRegisterFormCompanyService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست شرکت های ثبت نام", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] UserRegisterFormCompanyMainGrid searchInput)
        {
            return Json(UserRegisterFormCompanyService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] UserRegisterFormCompanyMainGrid searchInput)
        {
            var result = UserRegisterFormCompanyService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست فرم ثبت نام", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetFormList([FromQuery]int? cSOWSiteSettingId)
        {
            return Json(UserRegisterFormService.GetLightList(HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست فرم ثبت نام", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCompanyList()
        {
            return Json(CompanyService.GetLightList());
        }
    }
}
