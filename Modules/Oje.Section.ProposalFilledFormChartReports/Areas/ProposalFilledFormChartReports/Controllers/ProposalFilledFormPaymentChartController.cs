using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces;
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
    [AreaConfig(ModualTitle = "گزارش چارت فرم پیشنهاد", Icon = "fa-chart-pie", Title = "فرم های پیشنهاد پرداخت شده")]
    [CustomeAuthorizeFilter]
    public class ProposalFilledFormPaymentChartController: Controller
    {
        readonly IProposalFilledFormReportService ProposalFilledFormService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public ProposalFilledFormPaymentChartController
            (
                IProposalFilledFormReportService ProposalFilledFormService,
                ISiteSettingService SiteSettingService
            )
        {
            this.ProposalFilledFormService = ProposalFilledFormService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "فرم های پیشنهاد پرداخت شده", Icon = "fa-file-powerpoint", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "فرم های پیشنهاد پرداخت شده";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ProposalFilledFormPaymentChart", new { area = "ProposalFilledFormChartReports" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه فرم های پیشنهاد پرداخت شده", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("ProposalFilledFormChartReports", "ProposalFilledFormPaymentChart")));
        }

        [AreaConfig(Title = "فرم های پیشنهاد پرداخت شده", Icon = "fa-list-alt")]
        [HttpPost]
        public IActionResult GetSource()
        {
            return Json(ProposalFilledFormService.GetPaymentChartReport(SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId));
        }
    }
}
