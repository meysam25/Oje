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
    [AreaConfig(ModualTitle = "تنظیمات دبیر خانه", Icon = "fa-typewriter", Title = "متن ثابت نامه")]
    [CustomeAuthorizeFilter]
    public class SecretariatHeaderFooterDescriptionController: Controller
    {
        readonly ISecretariatHeaderFooterDescriptionService SecretariatHeaderFooterDescriptionService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public SecretariatHeaderFooterDescriptionController
            (
                ISecretariatHeaderFooterDescriptionService SecretariatHeaderFooterDescriptionService,
                ISiteSettingService SiteSettingService
            )
        {
            this.SecretariatHeaderFooterDescriptionService = SecretariatHeaderFooterDescriptionService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "متن ثابت نامه", Icon = "fa-file-alt", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "متن ثابت نامه";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "SecretariatHeaderFooterDescription", new { area = "SecretariatSupperAdmin" });

            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست متن ثابت نامه", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("SecretariatSupperAdmin", "SecretariatHeaderFooterDescription")));
        }

        [AreaConfig(Title = "افزودن متن ثابت نامه جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] SecretariatHeaderFooterDescriptionCreateUpdateVM input)
        {
            return Json(SecretariatHeaderFooterDescriptionService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف متن ثابت نامه", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(SecretariatHeaderFooterDescriptionService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک متن ثابت نامه", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(SecretariatHeaderFooterDescriptionService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  متن ثابت نامه", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] SecretariatHeaderFooterDescriptionCreateUpdateVM input)
        {
            return Json(SecretariatHeaderFooterDescriptionService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست متن ثابت نامه", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] SecretariatHeaderFooterDescriptionMainGrid searchInput)
        {
            return Json(SecretariatHeaderFooterDescriptionService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] SecretariatHeaderFooterDescriptionMainGrid searchInput)
        {
            var result = SecretariatHeaderFooterDescriptionService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
