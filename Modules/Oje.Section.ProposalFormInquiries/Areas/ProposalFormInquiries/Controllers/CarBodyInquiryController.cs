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
    [AreaConfig(ModualTitle = "استعلام", Icon = "fa-file-invoice", Title = "استعلام بدنه")]
    [CustomeAuthorizeFilter]
    public class CarBodyInquiryController: Controller
    {
        readonly ICompanyManager CompanyManager = null;
        readonly IInquiryDurationManager InquiryDurationManager = null;
        readonly AccountManager.Interfaces.ISiteSettingManager SiteSettingManager = null;
        readonly IProposalFormManager ProposalFormManager = null;
        readonly IInsuranceContractDiscountManager InsuranceContractDiscountManager = null;
        readonly IVehicleSystemManager VehicleSystemManager = null;
        readonly IVehicleTypeManager VehicleTypeManager = null;
        readonly IVehicleUsageManager VehicleUsageManager = null;
        readonly ICarExteraDiscountManager CarExteraDiscountManager = null;
        readonly ICarSpecificationAmountManager CarSpecificationAmountManager = null;
        readonly INoDamageDiscountManager NoDamageDiscountManager = null;
        public CarBodyInquiryController(
                ICompanyManager CompanyManager,
                IInquiryDurationManager InquiryDurationManager,
                AccountManager.Interfaces.ISiteSettingManager SiteSettingManager,
                IProposalFormManager ProposalFormManager,
                IInsuranceContractDiscountManager InsuranceContractDiscountManager,
                IVehicleSystemManager VehicleSystemManager,
                IVehicleTypeManager VehicleTypeManager,
                IVehicleUsageManager VehicleUsageManager,
                ICarExteraDiscountManager CarExteraDiscountManager,
                ICarSpecificationAmountManager CarSpecificationAmountManager,
                INoDamageDiscountManager NoDamageDiscountManager
            )
        {
            this.CompanyManager = CompanyManager;
            this.InquiryDurationManager = InquiryDurationManager;
            this.SiteSettingManager = SiteSettingManager;
            this.ProposalFormManager = ProposalFormManager;
            this.InsuranceContractDiscountManager = InsuranceContractDiscountManager;
            this.VehicleSystemManager = VehicleSystemManager;
            this.VehicleTypeManager = VehicleTypeManager;
            this.VehicleUsageManager = VehicleUsageManager;
            this.CarExteraDiscountManager = CarExteraDiscountManager;
            this.CarSpecificationAmountManager = CarSpecificationAmountManager;
            this.NoDamageDiscountManager = NoDamageDiscountManager;
        }

        [AreaConfig(Title = "استعلام بدنه", Icon = "fa-car-crash", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "استعلام بدنه";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "CarBodyInquiry", new { area = "ProposalFormInquiries" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه استعلام بدنه", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("ProposalFormInquiries", "CarBodyInquiry")));
        }

        [AreaConfig(Title = "استعلام بدنه", Icon = "fa-car-crash")]
        [HttpPost]
        public ActionResult Inquiry([FromForm] CarBodyInquiryVM input)
        {
            return Json(CarSpecificationAmountManager.Inquiry(SiteSettingManager.GetSiteSetting()?.Id, input));
        }

        [AreaConfig(Title = "مشاهده لیست شرکت ", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCompanyList()
        {
            return Json(CompanyManager.GetLightListForInquiryDD());
        }

        [AreaConfig(Title = "مشاهده لیست تخفیف عدم خسارت ", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetNoDamageDiscount()
        {
            return Json(NoDamageDiscountManager.GetLightList(ProposalFormManager.GetByType(ProposalFormType.CarBody, SiteSettingManager.GetSiteSetting()?.Id)?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست شرکت فیلتر گرید", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCompanyListGridFilter()
        {
            return Json(CompanyManager.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست بیمه نامه روزانه", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetValidDayForGridFilter()
        {
            return Json(InquiryDurationManager.GetLightList(SiteSettingManager.GetSiteSetting()?.Id, ProposalFormManager.GetByType(ProposalFormType.CarBody, SiteSettingManager.GetSiteSetting()?.Id)?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست تفاهم نامه ها", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetValidContractForGrid()
        {
            return Json(InsuranceContractDiscountManager.GetLightList(SiteSettingManager.GetSiteSetting()?.Id, ProposalFormType.CarBody));
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

        [AreaConfig(Title = "سوالات اجباری استعلام بدنه", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetRequiredQuestions([FromForm] RequiredQuestionVM input)
        {
            return Json(CarExteraDiscountManager.GetRequiredQuestions(input, ProposalFormManager.GetByType(ProposalFormType.CarBody, SiteSettingManager.GetSiteSetting()?.Id)?.Id));
        }


        [AreaConfig(Title = "سوالات اختیاری استعلام بدنه", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetExteraFilters([FromForm] RequiredQuestionVM input)
        {
            return Json(CarExteraDiscountManager.GetRequiredQuestionsJsonCtrls(input, ProposalFormManager.GetByType(ProposalFormType.CarBody, SiteSettingManager.GetSiteSetting()?.Id)?.Id));
        }

        [AreaConfig(Title = "مقادیر سوالات اختیاری استعلام بدنه", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetExteraFilterValuess([FromQuery] int? id)
        {
            return Json(CarExteraDiscountManager.GetValuesForDD(id.ToIntReturnZiro()));
        }
    }
}
