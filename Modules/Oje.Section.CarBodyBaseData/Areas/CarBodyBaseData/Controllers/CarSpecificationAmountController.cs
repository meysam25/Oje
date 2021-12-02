using Oje.AccountManager.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.CarBodyBaseData.Interfaces;
using Oje.Section.CarBodyBaseData.Models.View;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBodyBaseData.Areas.CarBodyBaseData.Controllers
{
    [Area("CarBodyBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = " پایه استعلام بدنه خودرو (ادمین)", Icon = "fa-car-crash", Title = "سرمایه بیمه بدنه")]
    [CustomeAuthorizeFilter]
    public class CarSpecificationAmountController: Controller
    {
        readonly ICompanyManager CompanyManager = null;
        readonly ICarSpecificationManager CarSpecificationManager = null;
        readonly ICarSpecificationAmountManager CarSpecificationAmountManager = null;
        public CarSpecificationAmountController(
                ICompanyManager CompanyManager,
                ICarSpecificationManager CarSpecificationManager,
                ICarSpecificationAmountManager CarSpecificationAmountManager
            )
        {
            this.CompanyManager = CompanyManager;
            this.CarSpecificationManager = CarSpecificationManager;
            this.CarSpecificationAmountManager = CarSpecificationAmountManager;
        }

        [AreaConfig(Title = "سرمایه بیمه بدنه", Icon = "fa-dollar-sign", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "سرمایه بیمه بدنه";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "CarSpecificationAmount", new { area = "CarBodyBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست سرمایه بیمه بدنه", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("CarBodyBaseData", "CarSpecificationAmount")));
        }

        [AreaConfig(Title = "افزودن سرمایه بیمه بدنه جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateCarSpecificationAmountVM input)
        {
            return Json(CarSpecificationAmountManager.Create(input));
        }

        [AreaConfig(Title = "حذف سرمایه بیمه بدنه", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(CarSpecificationAmountManager.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک سرمایه بیمه بدنه", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(CarSpecificationAmountManager.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  سرمایه بیمه بدنه", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateCarSpecificationAmountVM input)
        {
            return Json(CarSpecificationAmountManager.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست سرمایه بیمه بدنه", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] CarSpecificationAmountMainGrid searchInput)
        {
            return Json(CarSpecificationAmountManager.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] CarSpecificationAmountMainGrid searchInput)
        {
            var result = CarSpecificationAmountManager.GetList(searchInput);
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
    }
}
