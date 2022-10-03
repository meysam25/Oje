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
    [AreaConfig(ModualTitle = "ثبت نام کاربر", Icon = "fa-users", Title = "مدارک مورد نیاز ثبت نام")]
    [CustomeAuthorizeFilter]
    public class UserRegisterFormRequiredDocumentController: Controller
    {
        readonly IUserRegisterFormRequiredDocumentService UserRegisterFormRequiredDocumentService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IUserRegisterFormRequiredDocumentTypeService UserRegisterFormRequiredDocumentTypeService = null;

        public UserRegisterFormRequiredDocumentController(
                IUserRegisterFormRequiredDocumentService UserRegisterFormRequiredDocumentService,
                ISiteSettingService SiteSettingService,
                IUserRegisterFormRequiredDocumentTypeService UserRegisterFormRequiredDocumentTypeService
            )
        {
            this.UserRegisterFormRequiredDocumentService = UserRegisterFormRequiredDocumentService;
            this.SiteSettingService = SiteSettingService;
            this.UserRegisterFormRequiredDocumentTypeService = UserRegisterFormRequiredDocumentTypeService;
        }

        [AreaConfig(Title = "مدارک مورد نیاز ثبت نام", Icon = "fa-object-group", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "مدارک مورد نیاز ثبت نام";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "UserRegisterFormRequiredDocument", new { area = "RegisterFormAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه مدارک مورد نیاز ثبت نام", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("RegisterFormAdmin", "UserRegisterFormRequiredDocument")));
        }

        [AreaConfig(Title = "افزودن مدارک مورد نیاز ثبت نام جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] UserRegisterFormRequiredDocumentCreateUpdateVM input)
        {
            return Json(UserRegisterFormRequiredDocumentService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف مدارک مورد نیاز ثبت نام", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(UserRegisterFormRequiredDocumentService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک مدارک مورد نیاز ثبت نام", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(UserRegisterFormRequiredDocumentService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  مدارک مورد نیاز ثبت نام", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] UserRegisterFormRequiredDocumentCreateUpdateVM input)
        {
            return Json(UserRegisterFormRequiredDocumentService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست مدارک مورد نیاز ثبت نام", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] UserRegisterFormRequiredDocumentMainGrid searchInput)
        {
            return Json(UserRegisterFormRequiredDocumentService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] UserRegisterFormRequiredDocumentMainGrid searchInput)
        {
            var result = UserRegisterFormRequiredDocumentService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست فرم های ثبت نام", Icon = "fa-list-alt")]
        [HttpPost]
        public IActionResult GetTypeList([FromQuery]int? cSOWSiteSettingId)
        {
            return Json(UserRegisterFormRequiredDocumentTypeService.GetLightList(HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
