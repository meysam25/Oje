using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Section.SalesNetworkBaseData.Interfaces;
using Oje.Section.SalesNetworkBaseData.Models.View;

namespace Oje.Section.SalesNetworkBaseData.Areas.SalesNetworkBaseData.Controllers
{
    [Area("SalesNetworkBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات شبکه فروش/بازاریاب", Icon = "fa-network-wired", Title = "گزارش شبکه فروش")]
    [CustomeAuthorizeFilter]
    public class SalesNetworkReportChartController: Controller
    {
        readonly ISiteSettingService SiteSettingService = null;
        readonly ISalesNetworkService SalesNetworkService = null;

        public SalesNetworkReportChartController
            (
                ISiteSettingService SiteSettingService,
                ISalesNetworkService SalesNetworkService
            )
        {
            this.SiteSettingService = SiteSettingService;
            this.SalesNetworkService = SalesNetworkService;
        }

        [AreaConfig(Title = "گزارش شبکه فروش", Icon = "fa-chart-pie", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "گزارش شبکه فروش";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "SalesNetworkReportChart", new { area = "SalesNetworkBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه گزارش شبکه فروش", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("SalesNetworkBaseData", "SalesNetworkReportChart")));
        }

        [AreaConfig(Title = "گزارش شبکه فروش", Icon = "fa-list-alt")]
        [HttpPost]
        public IActionResult GetSource([FromForm] SalesNetworkReportMainGrid searchInput)
        {
            return Json(SalesNetworkService.GetReportChart(SiteSettingService.GetSiteSetting()?.Id, searchInput));
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
