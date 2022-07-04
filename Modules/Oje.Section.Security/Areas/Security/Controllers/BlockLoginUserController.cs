using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Security.Interfaces;
using Oje.Security.Models.View;
using System;

namespace Oje.Section.Security.Areas.Security.Controllers
{
    [Area("Security")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "امنیت", Icon = "fa-user-secret", Title = "قوانین محدود کردن ورود کاربر")]
    [CustomeAuthorizeFilter]
    public class BlockLoginUserController: Controller
    {
        readonly IBlockLoginUserService BlockLoginUserService = null;
        readonly ISiteSettingService SiteSettingService = null;

        public BlockLoginUserController
            (
                IBlockLoginUserService BlockLoginUserService,
                ISiteSettingService SiteSettingService
            )
        {
            this.BlockLoginUserService = BlockLoginUserService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "قوانین محدود کردن ورود کاربر", Icon = "fa-alarm-clock", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "قوانین محدود کردن ورود کاربر";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "BlockLoginUser", new { area = "Security" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست قوانین محدود کردن ورود کاربر", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Security", "BlockLoginUser")));
        }

        [AreaConfig(Title = "افزودن قوانین محدود کردن ورود کاربر جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] BlockLoginUserCreateUpdateVM input)
        {
            return Json(BlockLoginUserService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف قوانین محدود کردن ورود کاربر", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(BlockLoginUserService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک قوانین محدود کردن ورود کاربر", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(BlockLoginUserService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  قوانین محدود کردن ورود کاربر", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] BlockLoginUserCreateUpdateVM input)
        {
            return Json(BlockLoginUserService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست قوانین محدود کردن ورود کاربر", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] BlockLoginUserMainGrid searchInput)
        {
            return Json(BlockLoginUserService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] BlockLoginUserMainGrid searchInput)
        {
            var result = BlockLoginUserService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
