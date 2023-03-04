using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.FireInsuranceService.Interfaces;
using Oje.FireInsuranceService.Models.View;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Security.Interfaces;

namespace Oje.Section.ProposalFormInquiries.Areas.ProposalFormInquiries.Controllers
{
    [Area("ProposalFormInquiries")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "سامانه استعلام نرخ", Order = 6, Icon = "fa-file-invoice", Title = "استعلام آتش سوزی")]
    
    public class FireInsuranceController: Controller
    {
        readonly IFireInsuranceBuildingUnitValueService FireInsuranceBuildingUnitValueService = null;
        readonly IFireInsuranceBuildingTypeService FireInsuranceBuildingTypeService = null;
        readonly IFireInsuranceBuildingBodyService FireInsuranceBuildingBodyService = null;
        readonly IFireInsuranceBuildingAgeService FireInsuranceBuildingAgeService = null;
        readonly ICompanyService CompanyService = null;
        readonly IInquiryDurationService InquiryDurationService = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;
        readonly IProposalFormService ProposalFormService = null;
        readonly IInsuranceContractDiscountService InsuranceContractDiscountService = null;
        readonly IFireInsuranceRateService FireInsuranceRateService = null;
        readonly IFireInsuranceCoverageTitleService FireInsuranceCoverageTitleService = null;
        readonly IFireInsuranceTypeOfActivityService FireInsuranceTypeOfActivityService = null;
        readonly IBlockAutoIpService BlockAutoIpService = null;

        public FireInsuranceController(
                IFireInsuranceBuildingUnitValueService FireInsuranceBuildingUnitValueService,
                IFireInsuranceBuildingTypeService FireInsuranceBuildingTypeService,
                IFireInsuranceBuildingBodyService FireInsuranceBuildingBodyService,
                IFireInsuranceBuildingAgeService FireInsuranceBuildingAgeService,
                ICompanyService CompanyService,
                IInquiryDurationService InquiryDurationService,
                AccountService.Interfaces.ISiteSettingService SiteSettingService,
                IProposalFormService ProposalFormService,
                IInsuranceContractDiscountService InsuranceContractDiscountService,
                IFireInsuranceRateService FireInsuranceRateService,
                IFireInsuranceCoverageTitleService FireInsuranceCoverageTitleService,
                IFireInsuranceTypeOfActivityService FireInsuranceTypeOfActivityService,
                IBlockAutoIpService BlockAutoIpService
            )
        {
            this.FireInsuranceBuildingUnitValueService = FireInsuranceBuildingUnitValueService;
            this.FireInsuranceBuildingTypeService = FireInsuranceBuildingTypeService;
            this.FireInsuranceBuildingBodyService = FireInsuranceBuildingBodyService;
            this.FireInsuranceBuildingAgeService = FireInsuranceBuildingAgeService;
            this.CompanyService = CompanyService;
            this.InquiryDurationService = InquiryDurationService;
            this.SiteSettingService = SiteSettingService;
            this.ProposalFormService = ProposalFormService;
            this.InsuranceContractDiscountService = InsuranceContractDiscountService;
            this.FireInsuranceRateService = FireInsuranceRateService;
            this.FireInsuranceCoverageTitleService = FireInsuranceCoverageTitleService;
            this.FireInsuranceTypeOfActivityService = FireInsuranceTypeOfActivityService;
            this.BlockAutoIpService = BlockAutoIpService;
        }

        [AreaConfig(Title = "استعلام آتش سوزی", Icon = "fa-house-damage", IsMainMenuItem = true)]
        [HttpGet]
        [CustomeAuthorizeFilter]
        public IActionResult Index()
        {
            ViewBag.Title = "استعلام آتش سوزی";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "FireInsurance", new { area = "ProposalFormInquiries" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه استعلام آتش سوزی", Icon = "fa-cog")]
        [HttpPost]
        [HttpGet]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("ProposalFormInquiries", "FireInsurance")));
        }

        [AreaConfig(Title = "استعلام آتش سوزی", Icon = "fa-fire")]
        [HttpPost]
        public ActionResult Inquiry([FromForm] FireInsuranceInquiryVM input)
        {
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.FireInsuranceInquiry, BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            var tempResult = FireInsuranceRateService.Inquiry(SiteSettingService.GetSiteSetting()?.Id, input, Request.GetTargetAreaByRefferForInquiry());
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.FireInsuranceInquiry, BlockAutoIpAction.AfterExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);

            return Json(tempResult);
        }

        [AreaConfig(Title = "مشاهده لیست ارزش هر متر مربع", Icon = "fa-list-alt")]
        [HttpPost]
        public IActionResult GetBuildingUnitList()
        {
            return Json(FireInsuranceBuildingUnitValueService.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست نوع ساختمان", Icon = "fa-list-alt")]
        [HttpPost]
        public IActionResult GetBuildingTypeList()
        {
            return Json(FireInsuranceBuildingTypeService.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست نوع اسکلت", Icon = "fa-list-alt")]
        [HttpPost]
        public IActionResult GetBuildingBodyTypeList()
        {
            return Json(FireInsuranceBuildingBodyService.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست سن ساختمان", Icon = "fa-list-alt")]
        [HttpPost]
        public IActionResult GetBuildingAgeList()
        {
            return Json(FireInsuranceBuildingAgeService.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست شرکت فیلتر گرید", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCompanyListGridFilter()
        {
            return Json(CompanyService.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست بیمه نامه روزانه", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetValidDayForGridFilter()
        {
            return Json(InquiryDurationService.GetLightList(SiteSettingService.GetSiteSetting()?.Id, ProposalFormService.GetByType(ProposalFormType.FireInsurance, SiteSettingService.GetSiteSetting()?.Id)?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست تفاهم نامه ها", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetValidContractForGrid()
        {
            return Json(InsuranceContractDiscountService.GetLightList(SiteSettingService.GetSiteSetting()?.Id, ProposalFormType.FireInsurance));
        }

        [AreaConfig(Title = "مشاهده فیلتر اظافی", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetExteraFilters()
        {
            return Json(FireInsuranceCoverageTitleService.GetInquiryExteraFilterCtrls());
        }

        [AreaConfig(Title = "مشاهده لیست فعالیت ", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetActivityList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(FireInsuranceTypeOfActivityService.GetList(searchInput));
        }
    }
}
