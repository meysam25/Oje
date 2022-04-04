using Microsoft.AspNetCore.Mvc;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces;
using Oje.Section.WebMain.Models.View;

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
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "Proposal", input);
            return View("Index");
        }

        [HttpPost]
        [Route("[Controller]/[Action]/{fid}")]
        public IActionResult GetJsonConfig(string fid)
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(ProposalFormService.GetJSonConfigFile(fid.ToIntReturnZiro(), SiteSettingService.GetSiteSetting()?.Id));
        }

        [HttpGet]
        [Route("[Controller]/[Action]")]
        public IActionResult Detaile([FromQuery] long id, [FromQuery] bool isPrint = false)
        {
            ViewBag.isPrint = isPrint;
            ViewBag.newLayoutName = "_WebLayout";
            return View("PdfDetailesForAdmin", ProposalFilledFormAdminService.PdfDetailes(id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.New));
        }
    }
}
