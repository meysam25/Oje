using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Services;
using Oje.Section.WebMain.Interfaces;
using Oje.Section.WebMain.Models.View;
using Oje.Security.Interfaces;

namespace Oje.Section.WebMain.Controllers
{
    [Route("[Controller]/[Action]")]
    public class ReminderController : Controller
    {
        readonly IProposalFormReminderService ProposalFormReminderService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IPropertyService PropertyService = null;
        readonly IBlockAutoIpService BlockAutoIpService = null;

        public ReminderController
            (
                IProposalFormReminderService ProposalFormReminderService,
                ISiteSettingService SiteSettingService,
                IPropertyService PropertyService,
                IBlockAutoIpService BlockAutoIpService
            )
        {
            this.ProposalFormReminderService = ProposalFormReminderService;
            this.SiteSettingService = SiteSettingService;
            this.PropertyService = PropertyService;
            this.BlockAutoIpService = BlockAutoIpService;
        }

        [HttpPost]
        public ActionResult Create(ReminderCreateVM input)
        {
            BlockAutoIpService.CheckIfRequestIsValid(Infrastructure.Enums.BlockClientConfigType.CreateNewProposalFormReminder, Infrastructure.Enums.BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            var tempResult = ProposalFormReminderService.Create(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetIpAddress());
            BlockAutoIpService.CheckIfRequestIsValid(Infrastructure.Enums.BlockClientConfigType.CreateNewProposalFormReminder, Infrastructure.Enums.BlockAutoIpAction.AfterExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            return Json(tempResult);
        }

        [HttpPost]
        public ActionResult GetMainPageDescription()
        {
            return Json(PropertyService.GetBy<ReminderMainPageVM>(Infrastructure.Enums.PropertyType.RemindUsMainPage, SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
