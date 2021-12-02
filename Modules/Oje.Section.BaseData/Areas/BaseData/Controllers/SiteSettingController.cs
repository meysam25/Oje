using Oje.AccountManager.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.BaseData.Models.View;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Oje.Section.BaseData.Areas.BaseData.Controllers
{
    [Area("BaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = " پایه (ادمین)", Icon = "fa-archive", Title = "تنظیمات")]
    [CustomeAuthorizeFilter]
    public class SiteSettingController : Controller
    {
        readonly Interfaces.ISiteSettingManager SiteSettingManager = null;
        readonly Interfaces.IUserManager UserManager = null;

        public SiteSettingController(
                Interfaces.ISiteSettingManager SiteSettingManager,
                Interfaces.IUserManager UserManager
            )
        {
            this.SiteSettingManager = SiteSettingManager;
            this.UserManager = UserManager;
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
            return Json(SiteSettingManager.Create(input, HttpContext.GetLoginUserId()?.UserId));
        }

        [AreaConfig(Title = "حذف تنظیمات", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(SiteSettingManager.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک تنظیمات", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(SiteSettingManager.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  تنظیمات", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateSiteSettingVM input)
        {
            return Json(SiteSettingManager.Update(input, HttpContext.GetLoginUserId()?.UserId));
        }

        [AreaConfig(Title = "مشاهده لیست تنظیمات", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] SiteSettingMainGrid searchInput)
        {
            return Json(SiteSettingManager.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] SiteSettingMainGrid searchInput)
        {
            var result = SiteSettingManager.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست کاربران", Icon = "fa-list-alt ")]
        [HttpGet]
        public ActionResult GetUserList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(UserManager.GetSelect2List(searchInput));
        }
    }
}
