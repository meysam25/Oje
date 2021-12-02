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
    [AreaConfig(ModualTitle = " پایه استعلام آتش سوزی (ادمین)", Icon = "fa-fire", Title = "نرخ")]
    [CustomeAuthorizeFilter]
    public class FireInsuranceRateController: Controller
    {
        readonly IFireInsuranceRateManager FireInsuranceRateManager = null;
        readonly IFireInsuranceBuildingBodyManager FireInsuranceBuildingBodyManager = null;
        readonly IFireInsuranceBuildingTypeManager FireInsuranceBuildingTypeManager = null;
        readonly ICompanyManager CompanyManager = null;

        public FireInsuranceRateController(
                IFireInsuranceRateManager FireInsuranceRateManager,
                IFireInsuranceBuildingBodyManager FireInsuranceBuildingBodyManager,
                IFireInsuranceBuildingTypeManager FireInsuranceBuildingTypeManager,
                ICompanyManager CompanyManager
            )
        {
            this.FireInsuranceRateManager = FireInsuranceRateManager;
            this.FireInsuranceBuildingBodyManager = FireInsuranceBuildingBodyManager;
            this.FireInsuranceBuildingTypeManager = FireInsuranceBuildingTypeManager;
            this.CompanyManager = CompanyManager;
        }

        [AreaConfig(Title = "نرخ", Icon = "fa-comments-dollar", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "نرخ";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "FireInsuranceRate", new { area = "FireBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست نرخ", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("FireBaseData", "FireInsuranceRate")));
        }

        [AreaConfig(Title = "افزودن نرخ جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateFireInsuranceRateVM input)
        {
            return Json(FireInsuranceRateManager.Create(input));
        }

        [AreaConfig(Title = "حذف نرخ", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(FireInsuranceRateManager.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک نرخ", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(FireInsuranceRateManager.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  نرخ", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateFireInsuranceRateVM input)
        {
            return Json(FireInsuranceRateManager.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست نرخ", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] FireInsuranceRateMainGrid searchInput)
        {
            return Json(FireInsuranceRateManager.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] FireInsuranceRateMainGrid searchInput)
        {
            var result = FireInsuranceRateManager.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();
            
            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست شرکت", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetCompanyList()
        {
            return Json(CompanyManager.GetLightList());
        }

        [AreaConfig(Title = "مشاهده نوع ساختمان", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetBuildingTypeList()
        {
            return Json(FireInsuranceBuildingTypeManager.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست اسکلت ساختمان", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetBuildingBodyList()
        {
            return Json(FireInsuranceBuildingBodyManager.GetLightList());
        }
    }
}
