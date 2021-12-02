using Microsoft.AspNetCore.Mvc;
using Oje.AccountManager.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.ProposalFormManager.Interfaces;
using Oje.ProposalFormManager.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.ProposalFormInquiries.Areas.ProposalFormInquiries.Controllers
{
    [Area("ProposalFormInquiries")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "استعلام", Icon = "fa-file-invoice", Title = "استعلام ثالث")]
    [CustomeAuthorizeFilter]
    public class CarThirdPartyInquiryController : Controller
    {
        readonly ICompanyManager CompanyManager = null;
        readonly IVehicleSystemManager VehicleSystemManager = null;
        readonly IVehicleTypeManager VehicleTypeManager = null;
        readonly IVehicleUsageManager VehicleUsageManager = null;
        readonly IThirdPartyFinancialAndBodyHistoryDamagePenaltyManager ThirdPartyFinancialAndBodyHistoryDamagePenaltyManager = null;
        readonly IThirdPartyDriverHistoryDamagePenaltyManager ThirdPartyDriverHistoryDamagePenaltyManager = null;
        readonly AccountManager.Interfaces.ISiteSettingManager SiteSettingManager = null;
        readonly IThirdPartyDriverNoDamageDiscountHistoryManager ThirdPartyDriverNoDamageDiscountHistoryManager = null;
        readonly IThirdPartyRateManager ThirdPartyRateManager = null;
        readonly IThirdPartyBodyNoDamageDiscountHistoryManager ThirdPartyBodyNoDamageDiscountHistoryManager = null;
        readonly ICarExteraDiscountManager CarExteraDiscountManager = null;
        readonly IProposalFormManager ProposalFormManager = null;
        readonly IThirdPartyRequiredFinancialCommitmentManager ThirdPartyRequiredFinancialCommitmentManager = null;
        readonly IInquiryDurationManager InquiryDurationManager = null;
        readonly IInsuranceContractDiscountManager InsuranceContractDiscountManager = null;

        public CarThirdPartyInquiryController(
            ICompanyManager CompanyManager,
            IVehicleSystemManager VehicleSystemManager,
            IVehicleTypeManager VehicleTypeManager,
            IVehicleUsageManager VehicleUsageManager,
            IThirdPartyDriverHistoryDamagePenaltyManager ThirdPartyDriverHistoryDamagePenaltyManager,
            AccountManager.Interfaces.ISiteSettingManager SiteSettingManager,
            IThirdPartyDriverNoDamageDiscountHistoryManager ThirdPartyDriverNoDamageDiscountHistoryManager,
            IThirdPartyRateManager ThirdPartyRateManager,
            IThirdPartyFinancialAndBodyHistoryDamagePenaltyManager ThirdPartyFinancialAndBodyHistoryDamagePenaltyManager,
            IThirdPartyBodyNoDamageDiscountHistoryManager ThirdPartyBodyNoDamageDiscountHistoryManager,
            ICarExteraDiscountManager CarExteraDiscountManager,
            IProposalFormManager ProposalFormManager,
            IThirdPartyRequiredFinancialCommitmentManager ThirdPartyRequiredFinancialCommitmentManager,
            IInquiryDurationManager InquiryDurationManager,
            IInsuranceContractDiscountManager InsuranceContractDiscountManager
            )
        {
            this.CompanyManager = CompanyManager;
            this.VehicleSystemManager = VehicleSystemManager;
            this.VehicleTypeManager = VehicleTypeManager;
            this.VehicleUsageManager = VehicleUsageManager;
            this.ThirdPartyDriverHistoryDamagePenaltyManager = ThirdPartyDriverHistoryDamagePenaltyManager;
            this.SiteSettingManager = SiteSettingManager;
            this.ThirdPartyDriverNoDamageDiscountHistoryManager = ThirdPartyDriverNoDamageDiscountHistoryManager;
            this.ThirdPartyRateManager = ThirdPartyRateManager;
            this.ThirdPartyFinancialAndBodyHistoryDamagePenaltyManager = ThirdPartyFinancialAndBodyHistoryDamagePenaltyManager;
            this.ThirdPartyBodyNoDamageDiscountHistoryManager = ThirdPartyBodyNoDamageDiscountHistoryManager;
            this.CarExteraDiscountManager = CarExteraDiscountManager;
            this.ProposalFormManager = ProposalFormManager;
            this.ThirdPartyRequiredFinancialCommitmentManager = ThirdPartyRequiredFinancialCommitmentManager;
            this.InquiryDurationManager = InquiryDurationManager;
            this.InsuranceContractDiscountManager = InsuranceContractDiscountManager;
        }

        [AreaConfig(Title = "استعلام ثالث", Icon = "fa-car", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "استعلام ثالث";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "CarThirdPartyInquiry", new { area = "ProposalFormInquiries" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه استعلام ثالث", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("ProposalFormInquiries", "CarThirdPartyInquiry")));
        }

        [AreaConfig(Title = "استعلام ثالث", Icon = "fa-car")]
        [HttpPost]
        public ActionResult Inquiry([FromForm] CarThirdPartyInquiryVM input)
        {
            return Json(ThirdPartyRateManager.Inquiry(SiteSettingManager.GetSiteSetting()?.Id, input));
        }

        [AreaConfig(Title = "مشاهده لیست شرکت ", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCompanyList()
        {
            return Json(CompanyManager.GetLightListForInquiryDD());
        }

        [AreaConfig(Title = "مشاهده لیست شرکت فیلتر گرید", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCompanyListGridFilter()
        {
            return Json(CompanyManager.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست تعهد های مالی", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCommitmentGridFilter()
        {
            return Json(ThirdPartyRequiredFinancialCommitmentManager.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست بیمه نامه روزانه", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetValidDayForGridFilter()
        {
            return Json(InquiryDurationManager.GetLightList(SiteSettingManager.GetSiteSetting()?.Id, ProposalFormManager.GetByType(ProposalFormType.ThirdParty, SiteSettingManager.GetSiteSetting()?.Id)?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست تفاهم نامه ها", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetValidContractForGrid()
        {
            return Json(InsuranceContractDiscountManager.GetLightList(SiteSettingManager.GetSiteSetting()?.Id, ProposalFormType.ThirdParty));
        }

        [AreaConfig(Title = "مشاهده لیست برند خودرو", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetCarBrandList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(VehicleSystemManager.GetSelect2List(searchInput));
        }

        [AreaConfig(Title = "مشاهده لیست تیپ خودرو", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCarTypeList([FromForm] int? id)
        {
            return Json(VehicleTypeManager.GetLightList(id));
        }

        [AreaConfig(Title = "مشاهده لیست کاربری خودرو", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCarUsageList([FromForm] int? id)
        {
            return Json(VehicleUsageManager.GetLightList(id));
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
            return Json(ThirdPartyFinancialAndBodyHistoryDamagePenaltyManager.GetLightListForBody());
        }

        [AreaConfig(Title = "مشاهده لیست سابقه خسارت مالی خودرو", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetThirdPartyFinancialHistoryDamagePenaltyList()
        {
            return Json(ThirdPartyFinancialAndBodyHistoryDamagePenaltyManager.GetLightListForFinancial());
        }

        [AreaConfig(Title = "مشاهده لیست سابقه خسارت راننده خودرو", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetDriverDamageHistoryList()
        {
            return Json(ThirdPartyDriverHistoryDamagePenaltyManager.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست درصد عدم خسارت جانی", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetBodyNoDamageDiscountList()
        {
            return Json(ThirdPartyBodyNoDamageDiscountHistoryManager.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست درصد عدم خسارت راننده", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetDriverNoDamageDiscountHistoryList()
        {
            return Json(ThirdPartyDriverNoDamageDiscountHistoryManager.GetLightList());
        }

        [AreaConfig(Title = "سوالات اجباری استعلام ثالث", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetRequiredQuestions([FromForm] RequiredQuestionVM input)
        {
            return Json(CarExteraDiscountManager.GetRequiredQuestions(input, ProposalFormManager.GetByType(ProposalFormType.ThirdParty, SiteSettingManager.GetSiteSetting()?.Id)?.Id));
        }
    }
}
