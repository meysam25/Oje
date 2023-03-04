using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.View;
using Oje.Security.Interfaces;
using System;

namespace Oje.Section.ProposalFormInquiries.Areas.ProposalFormInquiries.Controllers
{
    [Area("ProposalFormInquiries")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "سامانه استعلام نرخ", Order = 6, Icon = "fa-file-invoice", Title = "استعلام بدنه")]
    public class CarBodyInquiryController: Controller
    {
        readonly ICompanyService CompanyService = null;
        readonly IInquiryDurationService InquiryDurationService = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;
        readonly IProposalFormService ProposalFormService = null;
        readonly IInsuranceContractDiscountService InsuranceContractDiscountService = null;
        readonly IVehicleSystemService VehicleSystemService = null;
        readonly IVehicleTypeService VehicleTypeService = null;
        readonly IVehicleUsageService VehicleUsageService = null;
        readonly ICarExteraDiscountService CarExteraDiscountService = null;
        readonly ICarSpecificationAmountService CarSpecificationAmountService = null;
        readonly INoDamageDiscountService NoDamageDiscountService = null;
        readonly ICarTypeService CarTypeService = null;
        readonly IVehicleSpecsService VehicleSpecsService = null;
        readonly IThirdPartyBodyNoDamageDiscountHistoryService ThirdPartyBodyNoDamageDiscountHistoryService = null;
        readonly IBlockAutoIpService BlockAutoIpService = null;

        public CarBodyInquiryController(
                ICompanyService CompanyService,
                IInquiryDurationService InquiryDurationService,
                AccountService.Interfaces.ISiteSettingService SiteSettingService,
                IProposalFormService ProposalFormService,
                IInsuranceContractDiscountService InsuranceContractDiscountService,
                IVehicleSystemService VehicleSystemService,
                IVehicleTypeService VehicleTypeService,
                IVehicleUsageService VehicleUsageService,
                ICarExteraDiscountService CarExteraDiscountService,
                ICarSpecificationAmountService CarSpecificationAmountService,
                INoDamageDiscountService NoDamageDiscountService,
                ICarTypeService CarTypeService,
                IVehicleSpecsService VehicleSpecsService,
                IThirdPartyBodyNoDamageDiscountHistoryService ThirdPartyBodyNoDamageDiscountHistoryService,
                IBlockAutoIpService BlockAutoIpService
            )
        {
            this.CompanyService = CompanyService;
            this.InquiryDurationService = InquiryDurationService;
            this.SiteSettingService = SiteSettingService;
            this.ProposalFormService = ProposalFormService;
            this.InsuranceContractDiscountService = InsuranceContractDiscountService;
            this.VehicleSystemService = VehicleSystemService;
            this.VehicleTypeService = VehicleTypeService;
            this.VehicleUsageService = VehicleUsageService;
            this.CarExteraDiscountService = CarExteraDiscountService;
            this.CarSpecificationAmountService = CarSpecificationAmountService;
            this.NoDamageDiscountService = NoDamageDiscountService;
            this.CarTypeService = CarTypeService;
            this.VehicleSpecsService = VehicleSpecsService;
            this.ThirdPartyBodyNoDamageDiscountHistoryService = ThirdPartyBodyNoDamageDiscountHistoryService;
            this.BlockAutoIpService = BlockAutoIpService;
        }

        [AreaConfig(Title = "استعلام بدنه", Icon = "fa-car-tilt", IsMainMenuItem = true)]
        [HttpGet]
        [CustomeAuthorizeFilter]
        public IActionResult Index()
        {
            ViewBag.Title = "استعلام بدنه";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "CarBodyInquiry", new { area = "ProposalFormInquiries" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه استعلام بدنه", Icon = "fa-cog")]
        [HttpPost]
        [HttpGet]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("ProposalFormInquiries", "CarBodyInquiry")));
        }

        [AreaConfig(Title = "استعلام بدنه", Icon = "fa-car-crash")]
        [HttpPost]
        public ActionResult Inquiry([FromForm] CarBodyInquiryVM input)
        {
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.CarBodyInquiry, BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            var tempResult = CarSpecificationAmountService.Inquiry(SiteSettingService.GetSiteSetting()?.Id, input, Request.GetTargetAreaByRefferForInquiry());
            BlockAutoIpService.CheckIfRequestIsValid(BlockClientConfigType.CarBodyInquiry, BlockAutoIpAction.AfterExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            return Json(tempResult);
        }

        [AreaConfig(Title = "مشاهده لیست شرکت ", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCompanyList()
        {
            return Json(CompanyService.GetLightListForInquiryDD());
        }

        [AreaConfig(Title = "مشاهده لیست تخفیف عدم خسارت ", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetNoDamageDiscount()
        {
            return Json(NoDamageDiscountService.GetLightList(ProposalFormService.GetByType(ProposalFormType.CarBody, SiteSettingService.GetSiteSetting()?.Id)?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست تخفیف عدم خسارت جانی ", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetNoDamageDiscountThirdParty()
        {
            return Json(ThirdPartyBodyNoDamageDiscountHistoryService.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست شرکت فیلتر گرید", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCompanyListGridFilter()
        {
            return Json(CompanyService.GetLightListForType(SiteSettingService.GetSiteSetting()?.Id, InquiryCompanyLimitType.CarBody));
        }

        [AreaConfig(Title = "مشاهده لیست بیمه نامه روزانه", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetValidDayForGridFilter()
        {
            return Json(InquiryDurationService.GetLightList(SiteSettingService.GetSiteSetting()?.Id, ProposalFormService.GetByType(ProposalFormType.CarBody, SiteSettingService.GetSiteSetting()?.Id)?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست تفاهم نامه ها", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetValidContractForGrid()
        {
            return Json(InsuranceContractDiscountService.GetLightList(SiteSettingService.GetSiteSetting()?.Id, ProposalFormType.CarBody));
        }

        [AreaConfig(Title = "مشاهده لیست برند خودرو", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetCarBrandList([FromQuery] Select2SearchVM searchInput, [FromQuery] int? vehicleTypeId)
        {
            return Json(VehicleSystemService.GetSelect2List(searchInput, vehicleTypeId));
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

        [AreaConfig(Title = "مشاهده لیست خصوصیات خودرو", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetCarSpecList([FromQuery] Select2SearchVM searchInput, [FromQuery] int? vehicleTypeId, [FromQuery] int? brandId)
        {
            return Json(VehicleSpecsService.GetSelect2List(searchInput, vehicleTypeId, brandId));
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

        [AreaConfig(Title = "سوالات اجباری استعلام بدنه", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetRequiredQuestions([FromForm] RequiredQuestionVM input)
        {
            return Json(CarExteraDiscountService.GetRequiredQuestions(input, ProposalFormService.GetByType(ProposalFormType.CarBody, SiteSettingService.GetSiteSetting()?.Id)?.Id));
        }


        [AreaConfig(Title = "سوالات اختیاری استعلام بدنه", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetExteraFilters([FromForm] RequiredQuestionVM input)
        {
            return Json(CarExteraDiscountService.GetRequiredQuestionsJsonCtrls(input, ProposalFormService.GetByType(ProposalFormType.CarBody, SiteSettingService.GetSiteSetting()?.Id)?.Id));
        }

        [AreaConfig(Title = "مقادیر سوالات اختیاری استعلام بدنه", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetExteraFilterValuess([FromQuery] int? id)
        {
            return Json(CarExteraDiscountService.GetValuesForDD(id.ToIntReturnZiro()));
        }
    }
}
