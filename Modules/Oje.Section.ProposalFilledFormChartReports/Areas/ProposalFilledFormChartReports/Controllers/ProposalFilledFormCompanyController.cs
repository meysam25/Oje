using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces.Reports;

namespace Oje.Section.ProposalFilledFormChartReports.Areas.ProposalFilledFormChartReports.Controllers
{
    [Area("ProposalFilledFormChartReports")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "گزارش چارت فرم پیشنهاد", Icon = "fa-chart-pie", Title = "تفکیک بر اساس شرکت بیمه")]
    [CustomeAuthorizeFilter]
    public class ProposalFilledFormCompanyController: Controller
    {
        readonly IProposalFilledFormReportService ProposalFilledFormService = null;
        readonly ISiteSettingService SiteSettingService = null;

        public ProposalFilledFormCompanyController
            (
                IProposalFilledFormReportService ProposalFilledFormService,
                ISiteSettingService SiteSettingService
            )
        {
            this.ProposalFilledFormService = ProposalFilledFormService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "تفکیک بر اساس شرکت بیمه", Icon = "fa-file-powerpoint", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "تفکیک بر اساس شرکت بیمه";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ProposalFilledFormCompany", new { area = "ProposalFilledFormChartReports" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه تفکیک بر اساس شرکت بیمه", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("ProposalFilledFormChartReports", "ProposalFilledFormCompany")));
        }

        [AreaConfig(Title = "تفکیک بر اساس شرکت بیمه", Icon = "fa-list-alt")]
        [HttpPost]
        public IActionResult GetSource()
        {
            return Json(ProposalFilledFormService.GetProposalFormCompanyChartReport(SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId));
        }
    }
}
