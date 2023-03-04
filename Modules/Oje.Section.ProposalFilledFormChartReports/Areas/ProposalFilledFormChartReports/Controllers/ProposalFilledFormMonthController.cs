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
    [AreaConfig(ModualTitle = "نمودار آمار و عملکرد", Order = 8, Icon = "fa-chart-pie", Title = "فروش ماهانه")]
    [CustomeAuthorizeFilter]
    public class ProposalFilledFormMonthController: Controller
    {
        readonly IProposalFilledFormReportService ProposalFilledFormService = null;
        readonly ISiteSettingService SiteSettingService = null;

        public ProposalFilledFormMonthController
            (
                 IProposalFilledFormReportService ProposalFilledFormService,
                 ISiteSettingService SiteSettingService
            )
        {
            this.ProposalFilledFormService = ProposalFilledFormService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "فروش ماهانه", Icon = "fa-file-powerpoint", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "فروش ماهانه";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ProposalFilledFormMonth", new { area = "ProposalFilledFormChartReports" });
            return View();
        }

        [AreaConfig(Title = "فروش ماهانه", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("ProposalFilledFormChartReports", "ProposalFilledFormMonth")));
        }

        [AreaConfig(Title = "فروش ماهانه", Icon = "fa-list-alt")]
        [HttpPost]
        public IActionResult GetSource()
        {
            return Json(ProposalFilledFormService.GetProposalFormTimeChartReport(SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, DateTime.Now.AddMonths(-1)));
        }
    }
}
