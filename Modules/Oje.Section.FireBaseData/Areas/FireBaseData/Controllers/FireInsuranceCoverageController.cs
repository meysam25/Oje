using Oje.AccountManager.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.FireBaseData.Interfaces;
using Oje.Section.FireBaseData.Models.View;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.FireBaseData.Areas.FireBaseData.Controllers
{
    [Area("FireBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = " پایه استعلام آتش سوزی (ادمین)", Icon = "fa-fire", Title = "نرخ پوشش")]
    [CustomeAuthorizeFilter]
    public class FireInsuranceCoverageController: Controller
    {
        readonly IFireInsuranceCoverageManager FireInsuranceCoverageManager = null;
        readonly ICompanyManager CompanyManager = null;
        readonly IFireInsuranceCoverageTitleManager FireInsuranceCoverageTitleManager = null;
        readonly IProposalFormManager ProposalFormManager = null;
        public FireInsuranceCoverageController(
                IFireInsuranceCoverageManager FireInsuranceCoverageManager,
                ICompanyManager CompanyManager,
                IFireInsuranceCoverageTitleManager FireInsuranceCoverageTitleManager,
                IProposalFormManager ProposalFormManager
            )
        {
            this.FireInsuranceCoverageManager = FireInsuranceCoverageManager;
            this.CompanyManager = CompanyManager;
            this.FireInsuranceCoverageTitleManager = FireInsuranceCoverageTitleManager;
            this.ProposalFormManager = ProposalFormManager;
        }

        [AreaConfig(Title = "نرخ پوشش", Icon = "fa-comments-dollar", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "نرخ پوشش";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "FireInsuranceCoverage", new { area = "FireBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست نرخ پوشش", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("FireBaseData", "FireInsuranceCoverage")));
        }

        [AreaConfig(Title = "افزودن نرخ پوشش جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateFireInsuranceCoverageVM input)
        {
            return Json(FireInsuranceCoverageManager.Create(input));
        }

        [AreaConfig(Title = "حذف نرخ پوشش", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(FireInsuranceCoverageManager.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک نرخ پوشش", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(FireInsuranceCoverageManager.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  نرخ پوشش", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateFireInsuranceCoverageVM input)
        {
            return Json(FireInsuranceCoverageManager.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست نرخ پوشش", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] FireInsuranceCoverageMainGrid searchInput)
        {
            return Json(FireInsuranceCoverageManager.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] FireInsuranceCoverageMainGrid searchInput)
        {
            var result = FireInsuranceCoverageManager.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست شرکت ", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetCompanyList()
        {
            return Json(CompanyManager.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست فرم های پیشنهاد", Icon = "fa-list-alt ")]
        [HttpGet]
        public ActionResult GetProposalFormList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(ProposalFormManager.GetSelect2List(searchInput));
        }

        [AreaConfig(Title = "مشاهده لیست پوشش ها", Icon = "fa-list-alt ")]
        [HttpGet]
        public ActionResult GetCoverTitleList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(FireInsuranceCoverageTitleManager.GetSelect2List(searchInput));
        }
    }
}
