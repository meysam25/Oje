using Oje.AccountManager.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.CarThirdBaseData.Interfaces;
using Oje.Section.CarThirdBaseData.Models.View;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarThirdBaseData.Areas.CarThirdBaseData.Controllers
{
    [Area("CarThirdBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = " پایه استعلام ثالث خودرو (ادمین)", Icon = "fa-car-side", Title = "نرخ تعهدات مازاد مالی")]
    [CustomeAuthorizeFilter]
    public class ThirdPartyExteraFinancialCommitmentController: Controller
    {
        readonly ICompanyManager CompanyManager = null;
        readonly IThirdPartyExteraFinancialCommitmentManager ThirdPartyExteraFinancialCommitmentManager = null;
        readonly IThirdPartyRequiredFinancialCommitmentManager ThirdPartyRequiredFinancialCommitmentManager = null;
        readonly ICarSpecificationManager CarSpecificationManager = null;
        public ThirdPartyExteraFinancialCommitmentController(
            ICompanyManager CompanyManager,
            IThirdPartyExteraFinancialCommitmentManager ThirdPartyExteraFinancialCommitmentManager,
            ICarSpecificationManager CarSpecificationManager,
            IThirdPartyRequiredFinancialCommitmentManager ThirdPartyRequiredFinancialCommitmentManager
            )
        {
            this.CompanyManager = CompanyManager;
            this.ThirdPartyExteraFinancialCommitmentManager = ThirdPartyExteraFinancialCommitmentManager;
            this.CarSpecificationManager = CarSpecificationManager;
            this.ThirdPartyRequiredFinancialCommitmentManager = ThirdPartyRequiredFinancialCommitmentManager;
        }

        [AreaConfig(Title = "نرخ تعهدات مازاد مالی", Icon = "fa-comments-dollar", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "نرخ تعهدات مازاد مالی";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ThirdPartyExteraFinancialCommitment", new { area = "CarThirdBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست نرخ تعهدات مازاد مالی", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("CarThirdBaseData", "ThirdPartyExteraFinancialCommitment")));
        }

        [AreaConfig(Title = "افزودن نرخ تعهدات مازاد مالی جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateThirdPartyExteraFinancialCommitmentVM input)
        {
            return Json(ThirdPartyExteraFinancialCommitmentManager.Create(input));
        }

        [AreaConfig(Title = "افزودن نرخ تعهدات مازاد مالی جدید از روی فایل اکسل", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult CreateFromXcel([FromForm] GlobalExcelFile input)
        {
            return Json(ThirdPartyExteraFinancialCommitmentManager.CreateFromExcel(input));
        }

        [AreaConfig(Title = "حذف نرخ تعهدات مازاد مالی", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(ThirdPartyExteraFinancialCommitmentManager.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک نرخ تعهدات مازاد مالی", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(ThirdPartyExteraFinancialCommitmentManager.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  نرخ تعهدات مازاد مالی", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateThirdPartyExteraFinancialCommitmentVM input)
        {
            return Json(ThirdPartyExteraFinancialCommitmentManager.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست نرخ تعهدات مازاد مالی", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] ThirdPartyExteraFinancialCommitmentMainGrid searchInput)
        {
            return Json(ThirdPartyExteraFinancialCommitmentManager.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ThirdPartyExteraFinancialCommitmentMainGrid searchInput)
        {
            var result = ThirdPartyExteraFinancialCommitmentManager.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست شرکت", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCompanyList()
        {
            return Json(CompanyManager.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست خصوصیات خودرو", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetCarSepecificationList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(CarSpecificationManager.GetSelect2List(searchInput));
        }

        [AreaConfig(Title = "مشاهده لیست تعهد مالی درخواستی (پوشش مالی)", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetThirdPartyRequiredFinancialCommitmentList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(ThirdPartyRequiredFinancialCommitmentManager.GetSelect2List(searchInput));
        }
    }
}
