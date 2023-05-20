using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
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
    [AreaConfig(ModualTitle = "پایه ثبت نام کاربر", Icon = "fa-users", Title = "توضیحات پرینت")]
    [CustomeAuthorizeFilter]
    public class UserRegisterFormPrintDescrptionController : Controller
    {
        readonly IUserRegisterFormService UserRegisterFormService = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;
        readonly IUserRegisterFormPrintDescrptionService UserRegisterFormPrintDescrptionService = null;

        public UserRegisterFormPrintDescrptionController(
                IUserRegisterFormService UserRegisterFormService,
                AccountService.Interfaces.ISiteSettingService SiteSettingService,
                IUserRegisterFormPrintDescrptionService UserRegisterFormPrintDescrptionService
            )
        {
            this.UserRegisterFormService = UserRegisterFormService;
            this.SiteSettingService = SiteSettingService;
            this.UserRegisterFormPrintDescrptionService = UserRegisterFormPrintDescrptionService;
        }

        [AreaConfig(Title = "توضیحات پرینت فرم پیشنهاد", Icon = "fa-print", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "توضیحات پرینت فرم پیشنهاد";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "UserRegisterFormPrintDescrption", new { area = "RegisterFormSupperAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست توضیحات پرینت فرم پیشنهاد", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("RegisterFormSupperAdmin", "UserRegisterFormPrintDescrption")));
        }

        [AreaConfig(Title = "افزودن توضیحات پرینت فرم پیشنهاد جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] UserRegisterFormPrintDescrptionCreateUpdateVM input)
        {
            return Json(UserRegisterFormPrintDescrptionService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف توضیحات پرینت فرم پیشنهاد", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(UserRegisterFormPrintDescrptionService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک توضیحات پرینت فرم پیشنهاد", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalLongId input)
        {
            return Json(UserRegisterFormPrintDescrptionService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  توضیحات پرینت فرم پیشنهاد", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] UserRegisterFormPrintDescrptionCreateUpdateVM input)
        {
            return Json(UserRegisterFormPrintDescrptionService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست توضیحات پرینت فرم پیشنهاد", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] UserRegisterFormPrintDescrptionMainGrid searchInput)
        {
            return Json(UserRegisterFormPrintDescrptionService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] UserRegisterFormPrintDescrptionMainGrid searchInput)
        {
            var result = UserRegisterFormPrintDescrptionService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست فرم های پیشنهاد", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetProposalFormList([FromQuery] int? cSOWSiteSettingId)
        {
            return Json(UserRegisterFormService.GetLightList(HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
