using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.SalesNetworkBaseData.Interfaces;
using Oje.Section.SalesNetworkBaseData.Models.View;
using System;

namespace Oje.Section.SalesNetworkBaseData.Areas.SalesNetworkBaseData.Controllers
{
    [Area("SalesNetworkBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات شبکه فروش/بازاریاب", Icon = "fa-network-wired", Title = "گزارش شبکه فروش")]
    [CustomeAuthorizeFilter]
    public class SalesNetworkReportController: Controller
    {
        readonly ISalesNetworkService SalesNetworkService = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;

        public SalesNetworkReportController
            (
                ISalesNetworkService SalesNetworkService,
                AccountService.Interfaces.ISiteSettingService SiteSettingService
            )
        {
            this.SalesNetworkService = SalesNetworkService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "گزارش شبکه فروش", Icon = "fa-project-diagram", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "گزارش شبکه فروش";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "SalesNetworkReport", new { area = "SalesNetworkBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه گزارش شبکه فروش", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("SalesNetworkBaseData", "SalesNetworkReport")));
        }

        [AreaConfig(Title = "مشاهده گزارش شبکه فروش", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] SalesNetworkReportMainGrid searchInput)
        {
            return Json(SalesNetworkService.GetReportList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] SalesNetworkReportMainGrid searchInput)
        {
            var result = SalesNetworkService.GetReportList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست شبکه های فروش", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetSalesNetworkList()
        {
            return Json(SalesNetworkService.GetLightListMultiLevel(SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست کاربران شبکه", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetUserList([FromQuery] Select2SearchVM searchInput, [FromQuery] int? snId)
        {
            return Json(SalesNetworkService.GetUserListBySaleNetworkId(SiteSettingService.GetSiteSetting()?.Id, snId, searchInput));
        }
    }
}
