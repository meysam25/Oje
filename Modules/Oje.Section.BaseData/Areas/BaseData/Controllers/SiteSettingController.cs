using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.BaseData.Models.View;
using Microsoft.AspNetCore.Mvc;
using System;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.BaseData.Areas.BaseData.Controllers
{
    [Area("BaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "پایه", Icon = "fa-archive", Title = "تنظیمات")]
    [CustomeAuthorizeFilter]
    public class SiteSettingController : Controller
    {
        readonly Interfaces.ISiteSettingService SiteSettingService = null;
        readonly Interfaces.IUserService UserService = null;

        public SiteSettingController(
                Interfaces.ISiteSettingService SiteSettingService,
                Interfaces.IUserService UserService
            )
        {
            this.SiteSettingService = SiteSettingService;
            this.UserService = UserService;
        }

        [AreaConfig(Title = "تنظیمات", Icon = "fa-wrench", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "تنظیمات";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "SiteSetting", new { area = "BaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست تنظیمات", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("BaseData", "SiteSetting")));
        }

        [AreaConfig(Title = "افزودن تنظیمات جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateSiteSettingVM input)
        {
            return Json(SiteSettingService.Create(input, HttpContext.GetLoginUser()?.UserId));
        }

        [AreaConfig(Title = "حذف تنظیمات", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(SiteSettingService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک تنظیمات", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(SiteSettingService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  تنظیمات", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateSiteSettingVM input)
        {
            return Json(SiteSettingService.Update(input, HttpContext.GetLoginUser()?.UserId));
        }

        [AreaConfig(Title = "مشاهده لیست تنظیمات", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] SiteSettingMainGrid searchInput)
        {
            return Json(SiteSettingService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] SiteSettingMainGrid searchInput)
        {
            var result = SiteSettingService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست کاربران", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetUserList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(UserService.GetSelect2List(searchInput));
        }
    }
}
