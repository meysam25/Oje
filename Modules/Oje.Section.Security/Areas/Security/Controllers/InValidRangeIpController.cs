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
    [AreaConfig(ModualTitle = "امنیت", Icon = "fa-user-secret", Title = "لیست ای پی های محدود شده")]
    [CustomeAuthorizeFilter]
    public class InValidRangeIpController: Controller
    {
        readonly IInValidRangeIpService InValidRangeIpService = null;
        readonly IErrorFirewallManualAddService ErrorFirewallManualAddService = null;
        readonly ISiteSettingService SiteSettingService = null;

        public InValidRangeIpController
            (
                IInValidRangeIpService InValidRangeIpService,
                IErrorFirewallManualAddService ErrorFirewallManualAddService,
                ISiteSettingService SiteSettingService
            )
        {
            this.InValidRangeIpService = InValidRangeIpService;
            this.ErrorFirewallManualAddService = ErrorFirewallManualAddService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "لیست ای پی های محدود شده", Icon = "fa-ban", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "لیست ای پی های محدود شده";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "InValidRangeIp", new { area = "Security" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست لیست ای پی های محدود شده", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Security", "InValidRangeIp")));
        }

        [AreaConfig(Title = "بلاک کردن در فایروال", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult BlockFirewall([FromForm] string id)
        {
            return Json(ErrorFirewallManualAddService.Block(id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست لیست ای پی های محدود شده", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] InValidRangeIpMainGrid searchInput)
        {
            return Json(InValidRangeIpService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] InValidRangeIpMainGrid searchInput)
        {
            var result = InValidRangeIpService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
