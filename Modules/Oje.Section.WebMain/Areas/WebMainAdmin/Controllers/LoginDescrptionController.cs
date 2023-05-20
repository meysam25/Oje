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
    [AreaConfig(ModualTitle = "تنظیمات وب سایت", Icon = "fa-browser", Title = "توضیحات ورود")]
    [CustomeAuthorizeFilter]
    public class LoginDescrptionController : Controller
    {
        readonly ISiteSettingService SiteSettingService = null;
        readonly Interfaces.ILoginDescrptionService LoginDescrptionService = null;
        public LoginDescrptionController
            (
                ISiteSettingService SiteSettingService,
                Interfaces.ILoginDescrptionService LoginDescrptionService
            )
        {
            this.SiteSettingService = SiteSettingService;
            this.LoginDescrptionService = LoginDescrptionService;
        }

        [AreaConfig(Title = "توضیحات ورود", Icon = "fa-sign-in", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "توضیحات ورود";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "LoginDescrption", new { area = "WebMainAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست توضیحات ورود", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("WebMainAdmin", "LoginDescrption")));
        }

        [AreaConfig(Title = "افزودن توضیحات ورود جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] LoginDescrptionCreateUpdateVM input)
        {
            return Json(LoginDescrptionService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف توضیحات ورود", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(LoginDescrptionService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک توضیحات ورود", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(LoginDescrptionService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  توضیحات ورود", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] LoginDescrptionCreateUpdateVM input)
        {
            return Json(LoginDescrptionService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست توضیحات ورود", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] LoginDescrptionMainGrid searchInput)
        {
            return Json(LoginDescrptionService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] LoginDescrptionMainGrid searchInput)
        {
            var result = LoginDescrptionService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
