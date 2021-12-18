using Oje.AccountService.Filters;
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
        readonly IFireInsuranceCoverageService FireInsuranceCoverageService = null;
        readonly ICompanyService CompanyService = null;
        readonly IFireInsuranceCoverageTitleService FireInsuranceCoverageTitleService = null;
        readonly IProposalFormService ProposalFormService = null;
        public FireInsuranceCoverageController(
                IFireInsuranceCoverageService FireInsuranceCoverageService,
                ICompanyService CompanyService,
                IFireInsuranceCoverageTitleService FireInsuranceCoverageTitleService,
                IProposalFormService ProposalFormService
            )
        {
            this.FireInsuranceCoverageService = FireInsuranceCoverageService;
            this.CompanyService = CompanyService;
            this.FireInsuranceCoverageTitleService = FireInsuranceCoverageTitleService;
            this.ProposalFormService = ProposalFormService;
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
            return Json(FireInsuranceCoverageService.Create(input));
        }

        [AreaConfig(Title = "حذف نرخ پوشش", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(FireInsuranceCoverageService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک نرخ پوشش", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(FireInsuranceCoverageService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  نرخ پوشش", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateFireInsuranceCoverageVM input)
        {
            return Json(FireInsuranceCoverageService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست نرخ پوشش", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] FireInsuranceCoverageMainGrid searchInput)
        {
            return Json(FireInsuranceCoverageService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] FireInsuranceCoverageMainGrid searchInput)
        {
            var result = FireInsuranceCoverageService.GetList(searchInput);
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
            return Json(CompanyService.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست فرم های پیشنهاد", Icon = "fa-list-alt ")]
        [HttpGet]
        public ActionResult GetProposalFormList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(ProposalFormService.GetSelect2List(searchInput));
        }

        [AreaConfig(Title = "مشاهده لیست پوشش ها", Icon = "fa-list-alt ")]
        [HttpGet]
        public ActionResult GetCoverTitleList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(FireInsuranceCoverageTitleService.GetSelect2List(searchInput));
        }
    }
}
