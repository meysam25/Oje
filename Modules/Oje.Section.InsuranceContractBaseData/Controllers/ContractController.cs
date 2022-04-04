using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Services;
using Oje.Section.InsuranceContractBaseData.Interfaces;
using Oje.Section.InsuranceContractBaseData.Models.View;
using Oje.Security.Interfaces;

namespace Oje.Section.InsuranceContractBaseData.Controllers
{
    [Route("[Controller]/[Action]")]
    public class ContractController : Controller
    {
        readonly IInsuranceContractService InsuranceContractService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IInsuranceContractTypeRequiredDocumentService InsuranceContractTypeRequiredDocumentService = null;
        readonly IBlockAutoIpService BlockAutoIpService = null;
        readonly IInsuranceContractProposalFilledFormService InsuranceContractProposalFilledFormService = null;

        public ContractController
            (
                IInsuranceContractService InsuranceContractService,
                ISiteSettingService SiteSettingService,
                IInsuranceContractTypeRequiredDocumentService InsuranceContractTypeRequiredDocumentService,
                IBlockAutoIpService BlockAutoIpService,
                IInsuranceContractProposalFilledFormService InsuranceContractProposalFilledFormService
            )
        {
            this.InsuranceContractService = InsuranceContractService;
            this.SiteSettingService = SiteSettingService;
            this.InsuranceContractTypeRequiredDocumentService = InsuranceContractTypeRequiredDocumentService;
            this.BlockAutoIpService = BlockAutoIpService;
            this.InsuranceContractProposalFilledFormService = InsuranceContractProposalFilledFormService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            GlobalServices.FillSeoInfo(
                 ViewData,
                  "ثبت خسارت",
                  "ثبت خسارت",
                  Request.Scheme + "://" + Request.Host + "/",
                  Request.Scheme + "://" + Request.Host + "/",
                  WebSiteTypes.website,
                  Request.Scheme + "://" + Request.Host + "/Modules/Assets/MainPage/logo.png",
                  null
                  );
            return View();
        }

        [HttpPost]
        public IActionResult GetStep1Config()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("InsuranceContractBaseDataWeb", "Contract")));
        }

        [HttpPost]
        public IActionResult GetJsonConfig([FromForm] contractUserInput input)
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(InsuranceContractService.GetFormJsonConfig(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [HttpPost]
        public IActionResult IsValid([FromForm] contractUserInput input)
        {
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.ContractValidationCheck, BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            var tempResult = InsuranceContractService.IsValid(input, SiteSettingService.GetSiteSetting()?.Id);
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.CreateProposalFilledForm, BlockAutoIpAction.AfterExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            return Json(tempResult);
        }

        [HttpPost]
        public IActionResult GetTermsHtml([FromForm] contractUserInput input)
        {
            var foundUserInfo = InsuranceContractService.GetTermsInfo(input, SiteSettingService.GetSiteSetting()?.Id);
            if (foundUserInfo == null)
                return NotFound();

            ViewBag.companyTitle = SiteSettingService.GetSiteSetting()?.Title;
            ViewBag.ContractFile = foundUserInfo.contractDocumentUrl;

            return View(foundUserInfo);
        }

        [HttpGet]
        public IActionResult Create([FromQuery] contractUserInput input)
        {
            InsuranceContractService.IsValid(input, SiteSettingService.GetSiteSetting()?.Id);

            var foundConteract = InsuranceContractService.GetByCode(input.contractCode, SiteSettingService.GetSiteSetting()?.Id);

            GlobalServices.FillSeoInfo(
                ViewData,
                 foundConteract.Title,
                 foundConteract.Description,
                 Request.Scheme + "://" + Request.Host + "/",
                 Request.Scheme + "://" + Request.Host + "/",
                 WebSiteTypes.website,
                 Request.Scheme + "://" + Request.Host + "/Modules/Assets/MainPage/logo.png",
                 null
                 );

            ViewBag.exteraParameters = input;
            return View();
        }

        [HttpPost]
        public IActionResult Create()
        {
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.ContractCreate, BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            var tempVar = InsuranceContractProposalFilledFormService.Create(HttpContext.GetLoginUser()?.UserId, SiteSettingService.GetSiteSetting()?.Id, Request.Form);
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.ContractCreate, BlockAutoIpAction.AfterExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            return Json(tempVar);
        }

        [HttpGet]
        public IActionResult Detaile([FromQuery] long? id, [FromQuery] bool isPrint = false)
        {
            ViewBag.isPrint = isPrint;
            ViewBag.newLayoutName = "_WebLayout";

            return View(InsuranceContractProposalFilledFormService.Detaile(id, HttpContext.GetLoginUser()?.UserId, SiteSettingService.GetSiteSetting()?.Id));
        }

        [HttpPost]
        public IActionResult GetFamilyMembers([FromForm] contractUserInput input)
        {
            return Json(InsuranceContractService.GetFamilyMemberList(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [HttpPost]
        public IActionResult GetContractTypeList([FromForm] contractUserInput input)
        {
            return Json(InsuranceContractService.GetContractTypeList(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [HttpPost]
        public IActionResult GetRequiredDocuments([FromForm] contractUserInput input, [FromForm] int? ctId)
        {
            return Json(InsuranceContractService.GetRequiredDocuments(input, ctId, SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
