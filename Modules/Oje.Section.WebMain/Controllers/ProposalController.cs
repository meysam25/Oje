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
            if (HttpContext.GetLoginUser()?.UserId == null || HttpContext.GetLoginUser()?.UserId <= 0)
                return RedirectToAction("Login", "Dashboard", new { area = "Account", returnUrl = Request.Path + Request.QueryString + "" });
            ViewBag.Title = "ثبت فرم";
            ViewBag.exteraParameters = input;
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "Proposal", input);
            ViewBag.fid = input?.fid;
            var curSetting = SiteSettingService.GetSiteSetting();
            if (curSetting?.WebsiteType == WebsiteType.Tender)
                ViewBag.layer = "~/Views/Shared/_TenderLayout.cshtml";

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
            ViewBag.IsMobile = Request.IsMobile();
            ViewBag.BaseUrl = "http" + (Request.IsHttps ? "s" : "") + "://" + Request.Host;
            return View("PdfDetailes", ProposalFilledFormAdminService.PdfDetailes(id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId, ProposalFilledFormStatus.New));
        }
    }
}
