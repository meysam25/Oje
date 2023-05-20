using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Secretariat.Interfaces;
using Oje.Section.Secretariat.Models.View;
using System;

namespace Oje.Section.Secretariat.Areas.SecretariatSupperAdmin.Controllers
{
    [Area("SecretariatSupperAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات دبیر خانه", Icon = "fa-typewriter", Title = "کاربران امضا کننده")]
    [CustomeAuthorizeFilter]
    public class SecretariatUserDigitalSignatureController: Controller
    {
        readonly ISiteSettingService SiteSettingService = null;
        readonly ISecretariatUserDigitalSignatureService SecretariatUserDigitalSignatureService = null;
        readonly Interfaces.IUserService UserService = null;
        public SecretariatUserDigitalSignatureController
            (
                ISiteSettingService SiteSettingService,
                ISecretariatUserDigitalSignatureService SecretariatUserDigitalSignatureService,
                Interfaces.IUserService UserService
            )
        {
            this.SiteSettingService = SiteSettingService;
            this.SecretariatUserDigitalSignatureService = SecretariatUserDigitalSignatureService;
            this.UserService = UserService;
        }

        [AreaConfig(Title = "کاربران امضا کننده", Icon = "fa-user-check", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "کاربران امضا کننده";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "SecretariatUserDigitalSignature", new { area = "SecretariatSupperAdmin" });

            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست کاربران امضا کننده", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("SecretariatSupperAdmin", "SecretariatUserDigitalSignature")));
        }

        [AreaConfig(Title = "افزودن کاربران امضا کننده جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] SecretariatUserDigitalSignatureCreateUpdateVM input)
        {
            return Json(SecretariatUserDigitalSignatureService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف کاربران امضا کننده", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(SecretariatUserDigitalSignatureService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک کاربران امضا کننده", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(SecretariatUserDigitalSignatureService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  کاربران امضا کننده", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] SecretariatUserDigitalSignatureCreateUpdateVM input)
        {
            return Json(SecretariatUserDigitalSignatureService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست کاربران امضا کننده", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] SecretariatUserDigitalSignatureMainGrid searchInput)
        {
            return Json(SecretariatUserDigitalSignatureService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] SecretariatUserDigitalSignatureMainGrid searchInput)
        {
            var result = SecretariatUserDigitalSignatureService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست کاربران", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetUsers([FromQuery] Select2SearchVM searchInput)
        {
            return Json(UserService.GetSelect2List(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
