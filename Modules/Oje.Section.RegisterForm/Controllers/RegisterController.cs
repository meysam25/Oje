using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.Section.RegisterForm.Interfaces;
using Oje.Security.Interfaces;

namespace Oje.Section.RegisterForm.Controllers
{
    public class RegisterController : Controller
    {
        readonly ISiteSettingService SiteSettingService = null;
        readonly IUserRegisterFormService UserRegisterFormService = null;
        readonly Interfaces.ICompanyService CompanyService = null;
        readonly IUserRegisterFormRequiredDocumentService UserRegisterFormRequiredDocumentService = null;
        readonly IBlockAutoIpService BlockAutoIpService = null;
        readonly IUserFilledRegisterFormService UserFilledRegisterFormService = null;

        public RegisterController(
                ISiteSettingService SiteSettingService,
                IUserRegisterFormService UserRegisterFormService,
                Interfaces.ICompanyService CompanyService,
                IUserRegisterFormRequiredDocumentService UserRegisterFormRequiredDocumentService,
                IBlockAutoIpService BlockAutoIpService,
                IUserFilledRegisterFormService UserFilledRegisterFormService
            )
        {
            this.SiteSettingService = SiteSettingService;
            this.UserRegisterFormService = UserRegisterFormService;
            this.CompanyService = CompanyService;
            this.UserRegisterFormRequiredDocumentService = UserRegisterFormRequiredDocumentService;
            this.BlockAutoIpService = BlockAutoIpService;
            this.UserFilledRegisterFormService = UserFilledRegisterFormService;
        }

        [Route("[Controller]/[Action]/{formName}")]
        [HttpGet]
        public IActionResult Users(string formName, int? fid)
        {
            var foundItem = UserRegisterFormService.GetBy(formName, fid, SiteSettingService.GetSiteSetting()?.Id);
            if (foundItem == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            GlobalServices.FillSeoInfo(
                    ViewData,
                     foundItem.Title,
                     foundItem.SeoDescription,
                     Request.Scheme + "://" + Request.Host + "/Register/Users/" + formName + "?fid=" + fid,
                     Request.Scheme + "://" + Request.Host + "/Register/Users/" + formName + "?fid=" + fid,
                     WebSiteTypes.website,
                     Request.Scheme + "://" + Request.Host + "/Modules/Assets/MainPage/logo.png",
                     null
                     );

            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "Register", new { fid = fid });
            ViewBag.exteraParameters = new { fid = fid };
            return View();
        }

        [HttpPost]
        public IActionResult Create()
        {
            if (HttpContext.GetLoginUser()?.UserId == null || HttpContext.GetLoginUser()?.UserId <= 0)
                throw BException.GenerateNewException(BMessages.Need_To_Be_Login_First);
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.CreateUserPreRegister, BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            var tempResult = UserFilledRegisterFormService.Create(SiteSettingService.GetSiteSetting()?.Id, Request.Form, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.UserId, HttpContext.GetLoginUser()?.UserId);
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.CreateUserPreRegister, BlockAutoIpAction.AfterExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            return Json(tempResult);
        }

        [HttpPost]
        [Route("[Controller]/[Action]")]
        public IActionResult GetJsonConfig([FromQuery] int? fid)
        {
            var strJson = UserRegisterFormService.GetConfigJson(fid, SiteSettingService.GetSiteSetting()?.Id);
            if (string.IsNullOrEmpty(strJson))
                throw BException.GenerateNewException(BMessages.Not_Found);
            Response.ContentType = "application/json; charset=utf-8";
            return Content(strJson);
        }

        [HttpPost]
        [Route("[Controller]/[Action]")]
        public IActionResult GetSecoundFileUrl([FromForm] int? fid)
        {
            return Content(UserRegisterFormService.GetSecoundFileUrl(fid, SiteSettingService.GetSiteSetting()?.Id));
        }

        [HttpGet]
        public IActionResult Details([FromQuery] long? id, [FromQuery] bool isPrint = false)
        {
            ViewBag.isPrint = isPrint;
            var result = UserFilledRegisterFormService.PdfDetailes(id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId);
            if (result == null)
                throw BException.GenerateNewException(BMessages.Not_Found);
            return View(result);
        }

        [HttpPost]
        public IActionResult GetCompanyList()
        {
            return Json(CompanyService.GetLightList());
        }

        [HttpPost]
        public IActionResult GetTermsHtml(int? fid)
        {
            var foundPPF = UserRegisterFormService.GetTermInfo(fid.ToIntReturnZiro(), SiteSettingService.GetSiteSetting()?.Id);
            if (foundPPF == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            ViewBag.RulesFile = foundPPF.RuleFile;
            ViewBag.HtmlTemplate = foundPPF.TermDescription;
            ViewBag.companyTitle = SiteSettingService.GetSiteSetting()?.Title;

            return View();
        }

        [HttpPost]
        public ActionResult GetRequiredDocuments([FromForm] int? fid)
        {
            return Json(UserRegisterFormRequiredDocumentService.GetLightList(SiteSettingService.GetSiteSetting()?.Id, fid));
        }
    }
}
