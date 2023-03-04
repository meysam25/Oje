using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.ProposalFilledFormChartReports.Areas.ProposalFilledFormChartReports.Controllers
{
    [Area("ProposalFilledFormChartReports")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "نمودار آمار و عملکرد", Order = 8, Icon = "fa-chart-pie", Title = "وضعیت فرم پیشنهاد")]
    [CustomeAuthorizeFilter]
    public class ProposalFilledFormStatusChartController: Controller
    {
        readonly IProposalFilledFormReportService ProposalFilledFormService = null;
        readonly ISiteSettingService SiteSettingService = null;

        public ProposalFilledFormStatusChartController
            (
                IProposalFilledFormReportService ProposalFilledFormService,
                ISiteSettingService SiteSettingService
            )
        {
            this.ProposalFilledFormService = ProposalFilledFormService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "وضعیت فرم پیشنهاد", Icon = "fa-file-powerpoint", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "وضعیت فرم پیشنهاد";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ProposalFilledFormStatusChart", new { area = "ProposalFilledFormChartReports" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه وضعیت فرم پیشنهاد", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("ProposalFilledFormChartReports", "ProposalFilledFormStatusChart")));
        }

        [AreaConfig(Title = "وضعیت فرم پیشنهاد", Icon = "fa-list-alt")]
        [HttpPost]
        public IActionResult GetSource()
        {
            return Json(ProposalFilledFormService.GetStatusChartReport(SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId));
        }
    }
}
