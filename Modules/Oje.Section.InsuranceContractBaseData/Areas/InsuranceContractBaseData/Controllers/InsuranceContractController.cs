using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.InsuranceContractBaseData.Interfaces;
using Oje.Section.InsuranceContractBaseData.Models.View;
using Microsoft.AspNetCore.Mvc;
using System;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.InsuranceContractBaseData.Areas.InsuranceContractBaseData.Controllers
{
    [Area("InsuranceContractBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "مدیریت قرارداد ها و مجوز ها", Icon = "fa-file-invoice", Title = "قرارداد (تفاهم نامه)")]
    [CustomeAuthorizeFilter]
    public class InsuranceContractController : Controller
    {
        readonly IInsuranceContractService InsuranceContractService = null;
        readonly IInsuranceContractCompanyService InsuranceContractCompanyService = null;
        readonly IInsuranceContractTypeService InsuranceContractTypeService = null;
        readonly IInsuranceContractProposalFormService InsuranceContractProposalFormService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly Interfaces.IProposalFormService ProposalFormService = null;

        public InsuranceContractController(
                IInsuranceContractService InsuranceContractService,
                IInsuranceContractCompanyService InsuranceContractCompanyService,
                IInsuranceContractTypeService InsuranceContractTypeService,
                IInsuranceContractProposalFormService InsuranceContractProposalFormService,
                ISiteSettingService SiteSettingService,
                Interfaces.IProposalFormService ProposalFormService
            )
        {
            this.InsuranceContractService = InsuranceContractService;
            this.InsuranceContractCompanyService = InsuranceContractCompanyService;
            this.InsuranceContractTypeService = InsuranceContractTypeService;
            this.InsuranceContractProposalFormService = InsuranceContractProposalFormService;
            this.SiteSettingService = SiteSettingService;
            this.ProposalFormService = ProposalFormService;
        }

        [AreaConfig(Title = "قرارداد (تفاهم نامه)", Icon = "fa-file-contract", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "قرارداد (تفاهم نامه)";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "InsuranceContract", new { area = "InsuranceContractBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست قرارداد (تفاهم نامه)", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("InsuranceContractBaseData", "InsuranceContract")));
        }

        [AreaConfig(Title = "افزودن قرارداد (تفاهم نامه) جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateInsuranceContractVM input)
        {
            return Json(InsuranceContractService.Create(input));
        }

        [AreaConfig(Title = "حذف قرارداد (تفاهم نامه)", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(InsuranceContractService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک قرارداد (تفاهم نامه)", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(InsuranceContractService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  قرارداد (تفاهم نامه)", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateInsuranceContractVM input)
        {
            return Json(InsuranceContractService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست قرارداد (تفاهم نامه)", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] InsuranceContractMainGrid searchInput)
        {
            return Json(InsuranceContractService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] InsuranceContractMainGrid searchInput)
        {
            var result = InsuranceContractService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);


            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده شرکت های بیمه گذار حقوقی", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetContractCompanyList([FromQuery] int? cSOWSiteSettingId)
        {
            return Json(InsuranceContractCompanyService.GetLightList(HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده نوع قراردادها", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetContractTypeList([FromQuery] int? cSOWSiteSettingId)
        {
            return Json(InsuranceContractTypeService.GetLightList(HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست فرم های پیشنهاد نمایشی", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetProposalFormList([FromQuery] Select2SearchVM searchInput, [FromQuery] int? cSOWSiteSettingId)
        {
            return Json(InsuranceContractProposalFormService.GetSelect2List(searchInput, HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست فرم های پیشنهاد", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetProposalFormRList([FromQuery] Select2SearchVM searchInput, [FromQuery] int? cSOWSiteSettingId)
        {
            return Json(ProposalFormService.GetSelect2List(searchInput, HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
