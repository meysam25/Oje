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
    [AreaConfig(ModualTitle = "نمودار آمار و عملکرد", Order = 8, Icon = "fa-chart-pie", Title = "فروش سالانه")]
    [CustomeAuthorizeFilter]
    public class ProposalFilledFormYearController: Controller
    {
        readonly IProposalFilledFormReportService ProposalFilledFormService = null;
        readonly ISiteSettingService SiteSettingService = null;

        public ProposalFilledFormYearController
            (
                IProposalFilledFormReportService ProposalFilledFormService,
                ISiteSettingService SiteSettingService
            )
        {
            this.SiteSettingService = SiteSettingService;
            this.ProposalFilledFormService = ProposalFilledFormService;
        }

        [AreaConfig(Title = "فروش سالانه", Icon = "fa-file-powerpoint", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "فروش سالانه";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ProposalFilledFormYear", new { area = "ProposalFilledFormChartReports" });
            return View();
        }

        [AreaConfig(Title = "فروش سالانه", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("ProposalFilledFormChartReports", "ProposalFilledFormYear")));
        }

        [AreaConfig(Title = "فروش سالانه", Icon = "fa-list-alt")]
        [HttpPost]
        public IActionResult GetSource()
        {
            return Json(ProposalFilledFormService.GetProposalFormTimeChartReport(SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, DateTime.Now.AddYears(-1)));
        }
    }
}
