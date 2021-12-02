using Oje.AccountManager.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.InsuranceContractBaseData.Interfaces;
using Oje.Section.InsuranceContractBaseData.Models.View;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Areas.InsuranceContractBaseData.Controllers
{
    [Area("InsuranceContractBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "مدیریت قرارداد ها و مجوز ها", Icon = "fa-file-invoice", Title = "قرارداد (تفاهم نامه)")]
    [CustomeAuthorizeFilter]
    public class InsuranceContractController: Controller
    {
        readonly IInsuranceContractManager InsuranceContractManager = null;
        readonly IInsuranceContractCompanyManager InsuranceContractCompanyManager = null;
        readonly IInsuranceContractTypeManager InsuranceContractTypeManager = null;
        readonly IProposalFormManager ProposalFormManager = null;
        public InsuranceContractController(
                IInsuranceContractManager InsuranceContractManager,
                IInsuranceContractCompanyManager InsuranceContractCompanyManager,
                IInsuranceContractTypeManager InsuranceContractTypeManager,
                IProposalFormManager ProposalFormManager
            )
        {
            this.InsuranceContractManager = InsuranceContractManager;
            this.InsuranceContractCompanyManager = InsuranceContractCompanyManager;
            this.InsuranceContractTypeManager = InsuranceContractTypeManager;
            this.ProposalFormManager = ProposalFormManager;
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
            return Json(InsuranceContractManager.Create(input));
        }

        [AreaConfig(Title = "حذف قرارداد (تفاهم نامه)", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(InsuranceContractManager.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک قرارداد (تفاهم نامه)", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(InsuranceContractManager.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  قرارداد (تفاهم نامه)", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateInsuranceContractVM input)
        {
            return Json(InsuranceContractManager.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست قرارداد (تفاهم نامه)", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] InsuranceContractMainGrid searchInput)
        {
            return Json(InsuranceContractManager.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] InsuranceContractMainGrid searchInput)
        {
            var result = InsuranceContractManager.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();


            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده شرکت های بیمه گذار حقوقی", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetContractCompanyList()
        {
            return Json(InsuranceContractCompanyManager.GetLightList());
        }

        [AreaConfig(Title = "مشاهده نوع قراردادها", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetContractTypeList()
        {
            return Json(InsuranceContractTypeManager.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست فرم های پیشنهاد", Icon = "fa-list-alt ")]
        [HttpGet]
        public ActionResult GetProposalFormList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(ProposalFormManager.GetSelect2List(searchInput));
        }
    }
}
