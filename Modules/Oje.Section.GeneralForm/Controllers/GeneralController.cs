using Microsoft.AspNetCore.Mvc;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Services;
using Oje.Section.GlobalForms.Interfaces;
using Oje.Security.Interfaces;
using System.Drawing;

namespace Oje.Section.GlobalForms.Controllers
{
    public class GeneralController : Controller
    {
        readonly IGeneralFormService GeneralFormService = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;
        readonly IBlockAutoIpService BlockAutoIpService = null;
        readonly IGeneralFilledFormService GeneralFilledFormService = null;
        readonly IGeneralFormRequiredDocumentService GeneralFormRequiredDocumentService = null;

        public GeneralController
            (
                IGeneralFormService GeneralFormService,
                AccountService.Interfaces.ISiteSettingService SiteSettingService,
                IBlockAutoIpService BlockAutoIpService,
                IGeneralFilledFormService GeneralFilledFormService,
                IGeneralFormRequiredDocumentService GeneralFormRequiredDocumentService
            )
        {
            this.GeneralFormService = GeneralFormService;
            this.SiteSettingService = SiteSettingService;
            this.BlockAutoIpService = BlockAutoIpService;
            this.GeneralFilledFormService = GeneralFilledFormService;
            this.GeneralFormRequiredDocumentService = GeneralFormRequiredDocumentService;
        }

        [Route("[Controller]/[Action]/{name}")]
        [HttpGet]
        public IActionResult Form([FromQuery] long? fid)
        {
            if (HttpContext.GetLoginUser()?.UserId == null || HttpContext.GetLoginUser()?.UserId <= 0)
                return RedirectToAction("Login", "Dashboard", new { area = "Account", returnUrl = Request.Path + Request.QueryString + "" });

            ViewBag.Title = "ثبت فرم";
            ViewBag.exteraParameters = new { fid = fid };
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "General", new { fid = fid });
            ViewBag.fid = fid;

            return View("Index");
        }

        [HttpPost]
        [Route("[Controller]/[Action]/{fid}")]
        public IActionResult GetJsonConfig(string fid)
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(GeneralFormService.GetJSonConfigFile(fid.ToIntReturnZiro(), SiteSettingService.GetSiteSetting()?.Id));
        }

        [HttpPost]
        public IActionResult Create()
        {
            var loginUser = HttpContext.GetLoginUser();
            if (loginUser == null || loginUser.UserId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.CreateGeneralForms, BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            var tempResult = GeneralFilledFormService.Create(SiteSettingService.GetSiteSetting()?.Id, Request.Form, loginUser);
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.CreateGeneralForms, BlockAutoIpAction.AfterExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            return Json(tempResult);
        }

        [HttpPost]
        public IActionResult GetTermsHtml([FromForm] int? fid)
        {
            var foundPPF = GeneralFormService.GetByIdNoTracking(fid.ToIntReturnZiro(), SiteSettingService.GetSiteSetting()?.Id);
            if (foundPPF == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            ViewBag.ContractFile = foundPPF.ContractFile;
            ViewBag.RulesFile = foundPPF.RulesFile;
            ViewBag.HtmlTemplate = foundPPF.TermTemplate;
            ViewBag.companyTitle = SiteSettingService.GetSiteSetting()?.Title;
            ViewBag.pTitle = foundPPF.Title;

            return View();
        }

        [HttpPost]
        public IActionResult GetRequiredDocument([FromForm] int? fid)
        {
            return Json(GeneralFormRequiredDocumentService.GetActiveForWeb(fid, SiteSettingService.GetSiteSetting()?.Id));
        }

        [HttpPost]
        public IActionResult PrintPreview()
        {
            var loginModel = HttpContext.GetLoginUser();
            var loginUserId = loginModel?.UserId;
            if (loginUserId.ToLongReturnZiro() <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            GeneralFilledFormService.createValidation(SiteSettingService.GetSiteSetting()?.Id, Request.Form, loginModel);
            ViewBag.makeLayoutNull = true;
            return View("Detaile", GeneralFilledFormService.PdfDetailesByForm(Request.Form, SiteSettingService.GetSiteSetting()?.Id));
        }

        [HttpGet]
        public IActionResult Detaile([FromQuery] long id, [FromQuery] bool isPrint = false)
        {
            ViewBag.isPrint = isPrint;
            ViewBag.newLayoutName = "~/Views/Shared/_WebLayout.cshtml";
            return View(GeneralFilledFormService.PdfDetailes(id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser(isPrint)?.UserId, null));
        }
    }
}
