using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Services;
using Oje.Section.WebMain.Interfaces;
using Oje.Section.WebMain.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Controllers
{
    [Route("[Controller]/[Action]")]
    public class ReminderController : Controller
    {
        readonly IProposalFormReminderService ProposalFormReminderService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IPropertyService PropertyService = null;
        public ReminderController
            (
                IProposalFormReminderService ProposalFormReminderService,
                ISiteSettingService SiteSettingService,
                IPropertyService PropertyService
            )
        {
            this.ProposalFormReminderService = ProposalFormReminderService;
            this.SiteSettingService = SiteSettingService;
            this.PropertyService = PropertyService;
        }

        [HttpPost]
        public ActionResult Create(ReminderCreateVM input)
        {
            return Json(ProposalFormReminderService.Create(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetIpAddress()));
        }

        [HttpPost]
        public ActionResult GetMainPageDescription()
        {
            return Json(PropertyService.GetBy<ReminderMainPageVM>(Infrastructure.Enums.PropertyType.RemindUsMainPage, SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
