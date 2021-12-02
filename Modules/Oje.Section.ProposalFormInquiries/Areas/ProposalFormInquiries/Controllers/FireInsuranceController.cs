using Microsoft.AspNetCore.Mvc;
using Oje.AccountManager.Filters;
using Oje.FireInsuranceManager.Interfaces;
using Oje.FireInsuranceManager.Models.View;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.ProposalFormInquiries.Areas.ProposalFormInquiries.Controllers
{
    [Area("ProposalFormInquiries")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "استعلام", Icon = "fa-file-invoice", Title = "استعلام آتش سوزی")]
    [CustomeAuthorizeFilter]
    public class FireInsuranceController: Controller
    {
        readonly IFireInsuranceBuildingUnitValueManager FireInsuranceBuildingUnitValueManager = null;
        readonly IFireInsuranceBuildingTypeManager FireInsuranceBuildingTypeManager = null;
        readonly IFireInsuranceBuildingBodyManager FireInsuranceBuildingBodyManager = null;
        readonly IFireInsuranceBuildingAgeManager FireInsuranceBuildingAgeManager = null;
        readonly ICompanyManager CompanyManager = null;
        readonly IInquiryDurationManager InquiryDurationManager = null;
        readonly AccountManager.Interfaces.ISiteSettingManager SiteSettingManager = null;
        readonly IProposalFormManager ProposalFormManager = null;
        readonly IInsuranceContractDiscountManager InsuranceContractDiscountManager = null;
        readonly IFireInsuranceRateManager FireInsuranceRateManager = null;
        readonly IFireInsuranceCoverageTitleManager FireInsuranceCoverageTitleManager = null;
        readonly IFireInsuranceTypeOfActivityManager FireInsuranceTypeOfActivityManager = null;

        public FireInsuranceController(
                IFireInsuranceBuildingUnitValueManager FireInsuranceBuildingUnitValueManager,
                IFireInsuranceBuildingTypeManager FireInsuranceBuildingTypeManager,
                IFireInsuranceBuildingBodyManager FireInsuranceBuildingBodyManager,
                IFireInsuranceBuildingAgeManager FireInsuranceBuildingAgeManager,
                ICompanyManager CompanyManager,
                IInquiryDurationManager InquiryDurationManager,
                AccountManager.Interfaces.ISiteSettingManager SiteSettingManager,
                IProposalFormManager ProposalFormManager,
                IInsuranceContractDiscountManager InsuranceContractDiscountManager,
                IFireInsuranceRateManager FireInsuranceRateManager,
                IFireInsuranceCoverageTitleManager FireInsuranceCoverageTitleManager,
                IFireInsuranceTypeOfActivityManager FireInsuranceTypeOfActivityManager
            )
        {
            this.FireInsuranceBuildingUnitValueManager = FireInsuranceBuildingUnitValueManager;
            this.FireInsuranceBuildingTypeManager = FireInsuranceBuildingTypeManager;
            this.FireInsuranceBuildingBodyManager = FireInsuranceBuildingBodyManager;
            this.FireInsuranceBuildingAgeManager = FireInsuranceBuildingAgeManager;
            this.CompanyManager = CompanyManager;
            this.InquiryDurationManager = InquiryDurationManager;
            this.SiteSettingManager = SiteSettingManager;
            this.ProposalFormManager = ProposalFormManager;
            this.InsuranceContractDiscountManager = InsuranceContractDiscountManager;
            this.FireInsuranceRateManager = FireInsuranceRateManager;
            this.FireInsuranceCoverageTitleManager = FireInsuranceCoverageTitleManager;
            this.FireInsuranceTypeOfActivityManager = FireInsuranceTypeOfActivityManager;
        }

        [AreaConfig(Title = "استعلام آتش سوزی", Icon = "fa-fire", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "استعلام آتش سوزی";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "FireInsurance", new { area = "ProposalFormInquiries" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه استعلام آتش سوزی", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("ProposalFormInquiries", "FireInsurance")));
        }

        [AreaConfig(Title = "استعلام آتش سوزی", Icon = "fa-fire")]
        [HttpPost]
        public ActionResult Inquiry([FromForm] FireInsuranceInquiryVM input)
        {
            return Json(FireInsuranceRateManager.Inquiry(SiteSettingManager.GetSiteSetting()?.Id, input));
        }

        [AreaConfig(Title = "مشاهده لیست ارزش هر متر مربع", Icon = "fa-list-alt")]
        [HttpPost]
        public IActionResult GetBuildingUnitList()
        {
            return Json(FireInsuranceBuildingUnitValueManager.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست نوع ساختمان", Icon = "fa-list-alt")]
        [HttpPost]
        public IActionResult GetBuildingTypeList()
        {
            return Json(FireInsuranceBuildingTypeManager.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست نوع اسکلت", Icon = "fa-list-alt")]
        [HttpPost]
        public IActionResult GetBuildingBodyTypeList()
        {
            return Json(FireInsuranceBuildingBodyManager.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست سن ساختمان", Icon = "fa-list-alt")]
        [HttpPost]
        public IActionResult GetBuildingAgeList()
        {
            return Json(FireInsuranceBuildingAgeManager.GetLightList());
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
            return Json(InquiryDurationManager.GetLightList(SiteSettingManager.GetSiteSetting()?.Id, ProposalFormManager.GetByType(ProposalFormType.FireInsurance, SiteSettingManager.GetSiteSetting()?.Id)?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست تفاهم نامه ها", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetValidContractForGrid()
        {
            return Json(InsuranceContractDiscountManager.GetLightList(SiteSettingManager.GetSiteSetting()?.Id, ProposalFormType.FireInsurance));
        }

        [AreaConfig(Title = "مشاهده فیلتر اظافی", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetExteraFilters()
        {
            return Json(FireInsuranceCoverageTitleManager.GetInquiryExteraFilterCtrls());
        }

        [AreaConfig(Title = "مشاهده لیست فعالیت ", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetActivityList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(FireInsuranceTypeOfActivityManager.GetList(searchInput));
        }
    }
}
