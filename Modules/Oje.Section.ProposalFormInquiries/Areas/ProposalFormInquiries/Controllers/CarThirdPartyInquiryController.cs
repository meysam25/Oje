using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.View;
using Oje.Sanab.Models.View;
using Oje.Security.Interfaces;
using System;
using System.Threading.Tasks;

namespace Oje.Section.ProposalFormInquiries.Areas.ProposalFormInquiries.Controllers
{
    [Area("ProposalFormInquiries")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "استعلام", Icon = "fa-file-invoice", Title = "استعلام ثالث")]
    public class CarThirdPartyInquiryController : Controller
    {
        readonly ICompanyService CompanyService = null;
        readonly IVehicleSystemService VehicleSystemService = null;
        readonly IVehicleTypeService VehicleTypeService = null;
        readonly IVehicleUsageService VehicleUsageService = null;
        readonly IThirdPartyFinancialAndBodyHistoryDamagePenaltyService ThirdPartyFinancialAndBodyHistoryDamagePenaltyService = null;
        readonly IThirdPartyDriverHistoryDamagePenaltyService ThirdPartyDriverHistoryDamagePenaltyService = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;
        readonly IThirdPartyDriverNoDamageDiscountHistoryService ThirdPartyDriverNoDamageDiscountHistoryService = null;
        readonly IThirdPartyRateService ThirdPartyRateService = null;
        readonly IThirdPartyBodyNoDamageDiscountHistoryService ThirdPartyBodyNoDamageDiscountHistoryService = null;
        readonly ICarExteraDiscountService CarExteraDiscountService = null;
        readonly IProposalFormService ProposalFormService = null;
        readonly IThirdPartyRequiredFinancialCommitmentService ThirdPartyRequiredFinancialCommitmentService = null;
        readonly IInquiryDurationService InquiryDurationService = null;
        readonly IInsuranceContractDiscountService InsuranceContractDiscountService = null;
        readonly ICarTypeService CarTypeService = null;
        readonly IVehicleSpecsService VehicleSpecsService = null;
        readonly IBlockAutoIpService BlockAutoIpService = null;
        readonly Sanab.Interfaces.ICarInquiry CarInquiry = null;
        readonly Sanab.Interfaces.ISanabCarThirdPartyPlaqueInquiryService SanabCarThirdPartyPlaqueInquiryService = null;

        public CarThirdPartyInquiryController(
            ICompanyService CompanyService,
            IVehicleSystemService VehicleSystemService,
            IVehicleTypeService VehicleTypeService,
            IVehicleUsageService VehicleUsageService,
            IThirdPartyDriverHistoryDamagePenaltyService ThirdPartyDriverHistoryDamagePenaltyService,
            AccountService.Interfaces.ISiteSettingService SiteSettingService,
            IThirdPartyDriverNoDamageDiscountHistoryService ThirdPartyDriverNoDamageDiscountHistoryService,
            IThirdPartyRateService ThirdPartyRateService,
            IThirdPartyFinancialAndBodyHistoryDamagePenaltyService ThirdPartyFinancialAndBodyHistoryDamagePenaltyService,
            IThirdPartyBodyNoDamageDiscountHistoryService ThirdPartyBodyNoDamageDiscountHistoryService,
            ICarExteraDiscountService CarExteraDiscountService,
            IProposalFormService ProposalFormService,
            IThirdPartyRequiredFinancialCommitmentService ThirdPartyRequiredFinancialCommitmentService,
            IInquiryDurationService InquiryDurationService,
            IInsuranceContractDiscountService InsuranceContractDiscountService,
            ICarTypeService CarTypeService,
            IVehicleSpecsService VehicleSpecsService,
            IBlockAutoIpService BlockAutoIpService,
            Sanab.Interfaces.ICarInquiry CarInquiry,
            Sanab.Interfaces.ISanabCarThirdPartyPlaqueInquiryService SanabCarThirdPartyPlaqueInquiryService
            )
        {
            this.BlockAutoIpService = BlockAutoIpService;
            this.CompanyService = CompanyService;
            this.VehicleSystemService = VehicleSystemService;
            this.VehicleTypeService = VehicleTypeService;
            this.VehicleUsageService = VehicleUsageService;
            this.ThirdPartyDriverHistoryDamagePenaltyService = ThirdPartyDriverHistoryDamagePenaltyService;
            this.SiteSettingService = SiteSettingService;
            this.ThirdPartyDriverNoDamageDiscountHistoryService = ThirdPartyDriverNoDamageDiscountHistoryService;
            this.ThirdPartyRateService = ThirdPartyRateService;
            this.ThirdPartyFinancialAndBodyHistoryDamagePenaltyService = ThirdPartyFinancialAndBodyHistoryDamagePenaltyService;
            this.ThirdPartyBodyNoDamageDiscountHistoryService = ThirdPartyBodyNoDamageDiscountHistoryService;
            this.CarExteraDiscountService = CarExteraDiscountService;
            this.ProposalFormService = ProposalFormService;
            this.ThirdPartyRequiredFinancialCommitmentService = ThirdPartyRequiredFinancialCommitmentService;
            this.InquiryDurationService = InquiryDurationService;
            this.InsuranceContractDiscountService = InsuranceContractDiscountService;
            this.CarTypeService = CarTypeService;
            this.VehicleSpecsService = VehicleSpecsService;
            this.CarInquiry = CarInquiry;
            this.SanabCarThirdPartyPlaqueInquiryService = SanabCarThirdPartyPlaqueInquiryService;
        }

        [AreaConfig(Title = "استعلام ثالث", Icon = "fa-car-crash", IsMainMenuItem = true)]
        [HttpGet]
        [CustomeAuthorizeFilter]
        public IActionResult Index()
        {
            ViewBag.Title = "استعلام ثالث";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "CarThirdPartyInquiry", new { area = "ProposalFormInquiries" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه استعلام ثالث", Icon = "fa-cog")]
        [HttpPost]
        [HttpGet]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("ProposalFormInquiries", "CarThirdPartyInquiry")));
        }

        [AreaConfig(Title = "استعلام ثالث", Icon = "fa-car")]
        [HttpPost]
        public ActionResult Inquiry([FromForm] CarThirdPartyInquiryVM input)
        {
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.CarThirdInquiry, BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            var tempResult = ThirdPartyRateService.Inquiry(SiteSettingService.GetSiteSetting()?.Id, input, Request.GetTargetAreaByRefferForInquiry());
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.CarThirdInquiry, BlockAutoIpAction.AfterExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            return Json(tempResult);
        }

        [AreaConfig(Title = "استعلام ثالث با پلاک", Icon = "fa-car")]
        [HttpPost]
        public async Task<ActionResult> PlaqueInquiry([FromForm] CarThirdPartyPlaqueVM input)
        {
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.CarThirdPlaqueInquiry, BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            var tempResult = await SanabCarThirdPartyPlaqueInquiryService.Discount(SiteSettingService.GetSiteSetting()?.Id, input, HttpContext.GetIpAddress());
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.CarThirdPlaqueInquiry, BlockAutoIpAction.AfterExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            return Json(tempResult);
        }

        [AreaConfig(Title = "مشاهده لیست شرکت ", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCompanyList()
        {
            return Json(CompanyService.GetLightListForInquiryDD());
        }

        [AreaConfig(Title = "مشاهده لیست شرکت فیلتر گرید", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCompanyListGridFilter()
        {
            return Json(CompanyService.GetLightListForType(SiteSettingService.GetSiteSetting()?.Id, InquiryCompanyLimitType.ThirdParty));
        }

        [AreaConfig(Title = "مشاهده لیست تعهد های مالی", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCommitmentGridFilter()
        {
            return Json(ThirdPartyRequiredFinancialCommitmentService.GetLightList(SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست تعهد های مالی عنوان کوتاه", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCommitmentGridFilterShortTitle()
        {
            return Json(ThirdPartyRequiredFinancialCommitmentService.GetLightListShortTitle(SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست بیمه نامه روزانه", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetValidDayForGridFilter()
        {
            return Json(InquiryDurationService.GetLightList(SiteSettingService.GetSiteSetting()?.Id, ProposalFormService.GetByType(ProposalFormType.ThirdParty, SiteSettingService.GetSiteSetting()?.Id)?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست تفاهم نامه ها", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetValidContractForGrid()
        {
            return Json(InsuranceContractDiscountService.GetLightList(SiteSettingService.GetSiteSetting()?.Id, ProposalFormType.ThirdParty));
        }

        [AreaConfig(Title = "مشاهده لیست برند خودرو", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetCarBrandList([FromQuery] Select2SearchVM searchInput, [FromQuery] int? vehicleTypeId)
        {
            return Json(VehicleSystemService.GetSelect2List(searchInput, vehicleTypeId));
        }

        [AreaConfig(Title = "مشاهده لیست خصوصیات خودرو", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetCarSpecList([FromQuery] Select2SearchVM searchInput, [FromQuery] int? vehicleTypeId, [FromQuery] int? brandId)
        {
            return Json(VehicleSpecsService.GetSelect2List(searchInput, vehicleTypeId, brandId));
        }

        [AreaConfig(Title = "مشاهده لیست نوع خودرو", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetVehicleTypeList()
        {
            return Json(VehicleTypeService.GetLightList());
        }

        [AreaConfig(Title = "مشاهده عنوان گروه بندی خصوصیت", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetSpecCatTitle([FromForm] int? vehicleTypeId)
        {
            return Json(VehicleTypeService.GetSpacCatTitleBy(vehicleTypeId));
        }

        [AreaConfig(Title = "مشاهده لیست کاربری خودرو", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCarTypeList([FromQuery] int id)
        {
            return Json(CarTypeService.GetLightList(id));
        }

        [AreaConfig(Title = "مشاهده لیست کاربری خودرو", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCarUsageList([FromForm] int? id)
        {
            return Json(VehicleUsageService.GetLightList(id));
        }

        [AreaConfig(Title = "مشاهده لیست سال ساخت خودرو", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCreateDateList()
        {
            return Json(DateTime.Now.FromYear(40));
        }

        [AreaConfig(Title = "مشاهده لیست سابقه خسارت جانی خودرو", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetThirdPartyBodyHistoryDamagePenaltyList()
        {
            return Json(ThirdPartyFinancialAndBodyHistoryDamagePenaltyService.GetLightListForBody());
        }

        [AreaConfig(Title = "مشاهده لیست سابقه خسارت مالی خودرو", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetThirdPartyFinancialHistoryDamagePenaltyList()
        {
            return Json(ThirdPartyFinancialAndBodyHistoryDamagePenaltyService.GetLightListForFinancial());
        }

        [AreaConfig(Title = "مشاهده لیست سابقه خسارت راننده خودرو", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetDriverDamageHistoryList()
        {
            return Json(ThirdPartyDriverHistoryDamagePenaltyService.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست درصد عدم خسارت جانی", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetBodyNoDamageDiscountList()
        {
            return Json(ThirdPartyBodyNoDamageDiscountHistoryService.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست درصد عدم خسارت راننده", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetDriverNoDamageDiscountHistoryList()
        {
            return Json(ThirdPartyDriverNoDamageDiscountHistoryService.GetLightList());
        }

        [AreaConfig(Title = "سوالات اجباری استعلام ثالث", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetRequiredQuestions([FromForm] RequiredQuestionVM input)
        {
            return Json(CarExteraDiscountService.GetRequiredQuestions(input, ProposalFormService.GetByType(ProposalFormType.ThirdParty, SiteSettingService.GetSiteSetting()?.Id)?.Id));
        }

        [AreaConfig(Title = "سوالات اختیاری استعلام ثالث", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetExteraFilters([FromForm] RequiredQuestionVM input)
        {
            return Json(CarExteraDiscountService.GetRequiredQuestionsJsonCtrls(input, ProposalFormService.GetByType(ProposalFormType.ThirdParty, SiteSettingService.GetSiteSetting()?.Id)?.Id));
        }

        [AreaConfig(Title = "مقادیر سوالات اختیاری استعلام ثالث", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetExteraFilterValuess([FromQuery] int? id)
        {
            return Json(CarExteraDiscountService.GetValuesForDD(id.ToIntReturnZiro()));
        }
    }
}
