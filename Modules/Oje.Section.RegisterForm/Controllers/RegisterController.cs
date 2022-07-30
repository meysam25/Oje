using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.PaymentService.Interfaces;
using Oje.Section.RegisterForm.Interfaces;
using Oje.Section.RegisterForm.Models.View;
using Oje.Security.Interfaces;
using System;

namespace Oje.Section.RegisterForm.Controllers
{
    public class RegisterController : Controller
    {
        readonly ISiteSettingService SiteSettingService = null;
        readonly IUserRegisterFormService UserRegisterFormService = null;
        readonly Interfaces.ICompanyService CompanyService = null;
        readonly IUserRegisterFormRequiredDocumentService UserRegisterFormRequiredDocumentService = null;
        readonly IBlockAutoIpService BlockAutoIpService = null;
        readonly Interfaces.IUserFilledRegisterFormService UserFilledRegisterFormService = null;
        readonly IAgentRefferService AgentRefferService = null;
        readonly IUserRegisterFormPriceService UserRegisterFormPriceService = null;
        readonly Interfaces.IUserService UserService = null;
        readonly IUserFilledRegisterFormCardPaymentService UserFilledRegisterFormCardPaymentService = null;
        readonly IPropertyService PropertyService = null;
        readonly IUserRegisterFormCompanyService UserRegisterFormCompanyService = null;
        readonly IUserRegisterFormPrintDescrptionService UserRegisterFormPrintDescrptionService = null;
        readonly IBankService BankService = null;

        public RegisterController(
                ISiteSettingService SiteSettingService,
                IUserRegisterFormService UserRegisterFormService,
                Interfaces.ICompanyService CompanyService,
                IUserRegisterFormRequiredDocumentService UserRegisterFormRequiredDocumentService,
                IBlockAutoIpService BlockAutoIpService,
                Interfaces.IUserFilledRegisterFormService UserFilledRegisterFormService,
                IAgentRefferService AgentRefferService,
                IUserRegisterFormPriceService UserRegisterFormPriceService,
                Interfaces.IUserService UserService,
                IUserFilledRegisterFormCardPaymentService UserFilledRegisterFormCardPaymentService,
                IPropertyService PropertyService,
                IUserRegisterFormCompanyService UserRegisterFormCompanyService,
                IUserRegisterFormPrintDescrptionService UserRegisterFormPrintDescrptionService,
                IBankService BankService
            )
        {
            this.SiteSettingService = SiteSettingService;
            this.UserRegisterFormService = UserRegisterFormService;
            this.CompanyService = CompanyService;
            this.UserRegisterFormRequiredDocumentService = UserRegisterFormRequiredDocumentService;
            this.BlockAutoIpService = BlockAutoIpService;
            this.UserFilledRegisterFormService = UserFilledRegisterFormService;
            this.AgentRefferService = AgentRefferService;
            this.UserRegisterFormPriceService = UserRegisterFormPriceService;
            this.UserService = UserService;
            this.UserFilledRegisterFormCardPaymentService = UserFilledRegisterFormCardPaymentService;
            this.PropertyService = PropertyService;
            this.UserRegisterFormCompanyService = UserRegisterFormCompanyService;
            this.UserRegisterFormPrintDescrptionService = UserRegisterFormPrintDescrptionService;
            this.BankService = BankService;
        }

        [Route("[Controller]/[Action]/{formName}")]
        [HttpGet]
        public IActionResult Users(string formName, int? fid)
        {
            if (HttpContext.GetLoginUser()?.UserId == null || HttpContext.GetLoginUser()?.UserId <= 0)
                return RedirectToAction("Login", "Dashboard", new { area = "Account", returnUrl = Request.Path + Request.QueryString + "" });

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
            ViewBag.fid = fid;
            return View();
        }

        [HttpPost]
        [Route("[Controller]/[Action]")]
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

        [HttpPost]
        [Route("[Controller]/[Action]")]
        public IActionResult GetAnotherFileUrl([FromForm] int? fid)
        {
            return Content(UserRegisterFormService.GetAnotherFileUrl(fid, SiteSettingService.GetSiteSetting()?.Id));
        }

        [HttpGet]
        [Route("[Controller]/[Action]")]
        public IActionResult DownloadPdf([FromQuery] GlobalLongId input)
        {
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.RegisterDownloadPDF, BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            var tempFile = HtmlToPdfBlink.Convert((Request.IsHttps ? "https" : "http") + "://" + Request.Host + Url.Action("Details", "Register", new { id = input.id, isPrint = true }), Request.Cookies);
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.RegisterDownloadPDF, BlockAutoIpAction.AfterExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            return File(
                    tempFile,
                    System.Net.Mime.MediaTypeNames.Application.Pdf, DateTime.Now.ToFaDate("_") + "_" + DateTime.Now.ToString("HH_mm_ss") + ".pdf"
                );
        }

        [HttpGet]
        [Route("[Controller]/[Action]")]
        public IActionResult Details([FromQuery] long? id, [FromQuery] bool isPrint = false)
        {
            ViewBag.isPrint = isPrint;
            var result = UserFilledRegisterFormService.PdfDetailes(id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser(isPrint)?.UserId, true);
            if (result == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return View(result);
        }

        [HttpPost]
        [Route("[Controller]/[Action]")]
        public IActionResult GetCompanyList([FromForm] int? fid)
        {
            return Json(UserRegisterFormCompanyService.GetLightList(fid, SiteSettingService.GetSiteSetting()?.Id));
        }

        [HttpPost]
        [Route("[Controller]/[Action]")]
        public IActionResult GetTermsHtml(int? fid, int? company)
        {
            var foundPPF = UserRegisterFormService.GetTermInfo(fid.ToIntReturnZiro(), SiteSettingService.GetSiteSetting()?.Id);
            if (foundPPF == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            ViewBag.RulesFile = foundPPF.RuleFile;
            ViewBag.HtmlTemplate = foundPPF.TermDescription;
            ViewBag.companyTitle = SiteSettingService.GetSiteSetting()?.Title;
            ViewBag.refferCode = AgentRefferService.GetRefferCode(SiteSettingService.GetSiteSetting()?.Id, company);

            return View();
        }

        [HttpPost]
        [Route("[Controller]/[Action]")]
        public ActionResult GetRequiredDocuments([FromForm] int? fid)
        {
            return Json(UserRegisterFormRequiredDocumentService.GetLightList(SiteSettingService.GetSiteSetting()?.Id, fid));
        }

        [HttpPost]
        [Route("[Controller]/[Action]")]
        public ActionResult GetPriceList([FromQuery] string id, [FromForm] int? fid)
        {
            return Json(UserRegisterFormPriceService.GetLightList(SiteSettingService.GetSiteSetting()?.Id, fid, id));
        }

        [HttpPost]
        [Route("[Controller]/[Action]")]
        public ActionResult GetUserInfo([FromForm] registerGetUserInfoVM input)
        {
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.GetUserInfoRegister, BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            var tempResult = UserService.GetUserInfo(SiteSettingService.GetSiteSetting()?.Id, input);
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.GetUserInfoRegister, BlockAutoIpAction.AfterExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            return Json(tempResult);
        }

        [HttpPost]
        [Route("[Controller]/[Action]")]
        public ActionResult CardPayment([FromForm] UserFilledRegisterFormCardPaymentCreateUpdateVM input)
        {
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.RegisterCardPayment, BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            var tempResult = UserFilledRegisterFormCardPaymentService.Create(SiteSettingService.GetSiteSetting()?.Id, input, HttpContext.GetLoginUser()?.UserId);
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.RegisterCardPayment, BlockAutoIpAction.AfterExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            return Json(tempResult);
        }

        [HttpPost]
        [Route("[Controller]/[Action]")]
        public ActionResult GetPaymentInfo()
        {
            return Json(PropertyService.GetBy<UserFilledRegisterFormTargetBankCardNoVM>(PropertyType.UserFilledRegisterFormTargetBankCardNo, SiteSettingService.GetSiteSetting()?.Id));
        }

        [HttpPost]
        [Route("[Controller]/[Action]")]
        public ActionResult GetBankList()
        {
            return Json(BankService.GetLightList());
        }
    }
}
