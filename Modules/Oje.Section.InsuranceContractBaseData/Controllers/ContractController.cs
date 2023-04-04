using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Services;
using Oje.Section.InsuranceContractBaseData.Interfaces;
using Oje.Section.InsuranceContractBaseData.Models.View;
using Oje.Security.Interfaces;
using Oje.Infrastructure.Exceptions;

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

            ViewBag.jsonConfigUrl = "/Contract/GetStep1Config";

            if (Request.IsMobile())
                return Json(new { configUrl = ViewBag.jsonConfigUrl });

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
            var curIp = HttpContext.GetIpAddress();
            var curSiteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.ContractValidationCheck, BlockAutoIpAction.BeforeExecute, curIp, curSiteSettingId);
            var tempResult = InsuranceContractService.IsValid(input, curSiteSettingId, curIp);
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.ContractValidationCheck, BlockAutoIpAction.AfterExecute, curIp, curSiteSettingId);
            return Json(tempResult);
        }


        [HttpPost]
        public IActionResult ConfirmSMS([FromForm] contractUserInput input)
        {
            var curIp = HttpContext.GetIpAddress();
            var curSiteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.ContractCreateConfirmSMS, BlockAutoIpAction.BeforeExecute, curIp, curSiteSettingId);
            var tempResult = InsuranceContractService.ConfirmSMSForCreate(input, curSiteSettingId, curIp);
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.ContractCreateConfirmSMS, BlockAutoIpAction.AfterExecute, curIp, curSiteSettingId);
            return Json(tempResult);
        }

        [HttpPost]
        public IActionResult IsValidSMS([FromForm] contractUserInput input)
        {
            var curIp = HttpContext.GetIpAddress();
            var curSiteSettingId = SiteSettingService.GetSiteSetting()?.Id;
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.ContractValidationSMSCheck, BlockAutoIpAction.BeforeExecute, curIp, curSiteSettingId);
            var tempResult = InsuranceContractService.IsValidSMS(input, curSiteSettingId, curIp);
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.ContractValidationSMSCheck, BlockAutoIpAction.AfterExecute, curIp, curSiteSettingId);
            return Json(tempResult);
        }

        [HttpPost]
        public IActionResult GetTermsHtml([FromForm] contractUserInput input)
        {
            var foundUserInfo = InsuranceContractService.GetTermsInfo(input, SiteSettingService.GetSiteSetting()?.Id);
            if (foundUserInfo == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            ViewBag.companyTitle = SiteSettingService.GetSiteSetting()?.Title;
            ViewBag.ContractFile = foundUserInfo.contractDocumentUrl;

            return View(foundUserInfo);
        }

        [HttpPost]
        public IActionResult GetPPFContractFile([FromForm] contractUserInput input)
        {
            var ss = SiteSettingService.GetSiteSetting();

            var foundPPF = InsuranceContractService.GetTermsInfo(input, ss?.Id);
            if (foundPPF == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Content("http" + (ss.IsHttps ? "s" : "") + "://" + ss?.WebsiteUrl +  foundPPF.contractDocumentUrl);
        }

        [HttpPost]
        public IActionResult Create([FromForm] contractUserInput input)
        {
            InsuranceContractService.IsValid(input, SiteSettingService.GetSiteSetting()?.Id, null);

            var foundConteract = InsuranceContractService.GetByCode(input, SiteSettingService.GetSiteSetting()?.Id);

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
        public IActionResult CreateNewItem()
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
