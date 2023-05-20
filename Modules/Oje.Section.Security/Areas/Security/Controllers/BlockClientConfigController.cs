using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Security.Interfaces;
using Oje.Security.Models.View;
using System;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.Security.Areas.Security.Controllers
{
    [Area("Security")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "امنیت", Icon = "fa-user-secret", Title = "قوانین محدود کردن کاربر")]
    [CustomeAuthorizeFilter]
    public class BlockClientConfigController : Controller
    {
        readonly IBlockClientConfigService BlockClientConfigService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public BlockClientConfigController
            (
                IBlockClientConfigService BlockClientConfigService,
                ISiteSettingService SiteSettingService
            )
        {
            this.BlockClientConfigService = BlockClientConfigService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "قوانین محدود کردن کاربر", Icon = "fa-shield-check", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "قوانین محدود کردن کاربر";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "BlockClientConfig", new { area = "Security" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست قوانین محدود کردن کاربر", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Security", "BlockClientConfig")));
        }

        [AreaConfig(Title = "افزودن قوانین محدود کردن کاربر جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] BlockClientConfigCreateUpdateVM input)
        {
            return Json(BlockClientConfigService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف قوانین محدود کردن کاربر", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(BlockClientConfigService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک قوانین محدود کردن کاربر", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(BlockClientConfigService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  قوانین محدود کردن کاربر", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] BlockClientConfigCreateUpdateVM input)
        {
            return Json(BlockClientConfigService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست قوانین محدود کردن کاربر", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] BlockClientConfigMainGrid searchInput)
        {
            return Json(BlockClientConfigService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] BlockClientConfigMainGrid searchInput)
        {
            var result = BlockClientConfigService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
