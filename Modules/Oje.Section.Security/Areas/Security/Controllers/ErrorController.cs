using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Services;
using Oje.Security.Interfaces;
using Oje.Security.Models.View;
using System;

namespace Oje.Section.Security.Areas.Security.Controllers
{
    [Area("Security")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "امنیت", Icon = "fa-user-secret", Title = "لاگ خطا ها")]
    [CustomeAuthorizeFilter]
    public class ErrorController : Controller
    {
        readonly IErrorService ErrorService = null;
        readonly IErrorFirewallManualAddService ErrorFirewallManualAddService = null;
        readonly ISiteSettingService SiteSettingService = null;

        public ErrorController
            (
                IErrorService ErrorService,
                IErrorFirewallManualAddService ErrorFirewallManualAddService,
                ISiteSettingService SiteSettingService
            )
        {
            this.ErrorService = ErrorService;
            this.SiteSettingService = SiteSettingService;
            this.ErrorFirewallManualAddService = ErrorFirewallManualAddService;
        }

        [AreaConfig(Title = "لاگ خطا ها", Icon = "fa-bug", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "لاگ خطا ها";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "Error", new { area = "Security" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست لاگ خطا ها", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Security", "Error")));
        }

        [AreaConfig(Title = "مشاهده جزئیات خطای لاگ", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] long? id)
        {
            return Json(ErrorService.GetParameters(id));
        }

        [AreaConfig(Title = "بلاک کردن در فایروال", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult BlockFirewall([FromForm] string id)
        {
            return Json(ErrorFirewallManualAddService.Block(id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست لاگ خطا ها", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] ErrorMainGrid searchInput)
        {
            return Json(ErrorService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ErrorMainGrid searchInput)
        {
            var result = ErrorService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
