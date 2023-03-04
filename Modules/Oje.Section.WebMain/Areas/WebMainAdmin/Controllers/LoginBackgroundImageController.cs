using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.WebMain.Models.View;
using System;

namespace Oje.Section.WebMain.Areas.WebMainAdmin.Controllers
{
    [Area("WebMainAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات وب سایت", Icon = "fa-browser", Title = "تصویر پس زمینه ورود")]
    [CustomeAuthorizeFilter]
    public class LoginBackgroundImageController : Controller
    {
        readonly ISiteSettingService SiteSettingService = null;
        readonly Interfaces.ILoginBackgroundImageService LoginBackgroundImageService = null;

        public LoginBackgroundImageController
            (
                ISiteSettingService SiteSettingService,
                Interfaces.ILoginBackgroundImageService LoginBackgroundImageService
            )
        {
            this.SiteSettingService = SiteSettingService;
            this.LoginBackgroundImageService = LoginBackgroundImageService;
        }

        [AreaConfig(Title = "تصویر پس زمینه ورود", Icon = "fa-image", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "تصویر پس زمینه ورود";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "LoginBackgroundImage", new { area = "WebMainAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست تصویر پس زمینه ورود", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("WebMainAdmin", "LoginBackgroundImage")));
        }

        [AreaConfig(Title = "افزودن تصویر پس زمینه ورود جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] LoginBackgroundImageCreateUpdateVM input)
        {
            return Json(LoginBackgroundImageService.Create(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId));
        }

        [AreaConfig(Title = "حذف تصویر پس زمینه ورود", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(LoginBackgroundImageService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک تصویر پس زمینه ورود", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(LoginBackgroundImageService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  تصویر پس زمینه ورود", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] LoginBackgroundImageCreateUpdateVM input)
        {
            return Json(LoginBackgroundImageService.Update(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId));
        }

        [AreaConfig(Title = "مشاهده لیست تصویر پس زمینه ورود", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] LoginBackgroundImageMainGrid searchInput)
        {
            return Json(LoginBackgroundImageService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] LoginBackgroundImageMainGrid searchInput)
        {
            var result = LoginBackgroundImageService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
