using Microsoft.AspNetCore.Mvc;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces;
using Oje.Section.WebMain.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Controllers
{
    public class ProposalController: Controller
    {
        readonly IProposalFormService ProposalFormService = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;
        readonly IProposalFilledFormAdminService ProposalFilledFormAdminService = null;
        public ProposalController(
            IProposalFormService ProposalFormService,
            AccountService.Interfaces.ISiteSettingService SiteSettingService,
            IProposalFilledFormAdminService ProposalFilledFormAdminService
            )
        {
            this.ProposalFormService = ProposalFormService;
            this.SiteSettingService = SiteSettingService;
            this.ProposalFilledFormAdminService = ProposalFilledFormAdminService;
        }

        [Route("[Controller]/[Action]")]
        [HttpGet]
        public IActionResult Form([FromQuery] ProposalFormVM input)
        {
            ViewBag.Title = "ثبت فرم";
            ViewBag.exteraParameters = input;
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "Proposal");
            return View("Index");
        }

        [HttpPost]
        public IActionResult GetJsonConfig([FromForm] ProposalFormVM input)
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(ProposalFormService.GetJSonConfigFile(input.fid.ToIntReturnZiro(), SiteSettingService.GetSiteSetting()?.Id));
        }

        [HttpGet]
        public IActionResult Detaile([FromQuery] int id, [FromQuery] bool isPrint = false)
        {
            ViewBag.isPrint = isPrint;
            ViewBag.newLayoutName = "_WebLayout";
            return View("PdfDetailesForAdmin", ProposalFilledFormAdminService.PdfDetailes(id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUserId()?.UserId, ProposalFilledFormStatus.New));
        }
    }
}
