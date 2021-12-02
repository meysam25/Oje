using Oje.AccountManager.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.CarBaseData.Interfaces;
using Oje.Section.CarBaseData.Models.View;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Areas.CarBaseData.Controllers
{
    [Area("CarBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = " پایه خودرو (ادمین)", Icon = "fa-car", Title = "نرخ تخفیف اضافه")]
    [CustomeAuthorizeFilter]
    public class CarExteraDiscountRangeAmountController : Controller
    {
        readonly ICarExteraDiscountRangeAmountManager CarExteraDiscountRangeAmountManager = null;
        readonly ICompanyManager CompanyManager = null;
        readonly ICarExteraDiscountManager CarExteraDiscountManager = null;
        readonly ICarExteraDiscountValueManager CarExteraDiscountValueManager = null;
        public CarExteraDiscountRangeAmountController(
            ICarExteraDiscountRangeAmountManager CarExteraDiscountRangeAmountManager,
            ICompanyManager CompanyManager,
            ICarExteraDiscountManager CarExteraDiscountManager,
            ICarExteraDiscountValueManager CarExteraDiscountValueManager
            )
        {
            this.CarExteraDiscountRangeAmountManager = CarExteraDiscountRangeAmountManager;
            this.CompanyManager = CompanyManager;
            this.CarExteraDiscountManager = CarExteraDiscountManager;
            this.CarExteraDiscountValueManager = CarExteraDiscountValueManager;
        }

        [AreaConfig(Title = "نرخ تخفیف اضافه", Icon = "fa-money-bill-wave", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "نرخ تخفیف اضافه";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "CarExteraDiscountRangeAmount", new { area = "CarBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست نرخ تخفیف اضافه", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("CarBaseData", "CarExteraDiscountRangeAmount")));
        }

        [AreaConfig(Title = "افزودن نرخ تخفیف اضافه جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateCarExteraDiscountRangeAmountVM input)
        {
            return Json(CarExteraDiscountRangeAmountManager.Create(input));
        }

        [AreaConfig(Title = "حذف نرخ تخفیف اضافه", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(CarExteraDiscountRangeAmountManager.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک نرخ تخفیف اضافه", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(CarExteraDiscountRangeAmountManager.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  نرخ تخفیف اضافه", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateCarExteraDiscountRangeAmountVM input)
        {
            return Json(CarExteraDiscountRangeAmountManager.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست نرخ تخفیف اضافه", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] CarExteraDiscountRangeAmountMainGrid searchInput)
        {
            return Json(CarExteraDiscountRangeAmountManager.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] CarExteraDiscountRangeAmountMainGrid searchInput)
        {
            var result = CarExteraDiscountRangeAmountManager.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست شرکت بیمه", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCompanyList()
        {
            return Json(CompanyManager.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست تخفیف اظافه", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetCarExteraDiscountList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(CarExteraDiscountManager.GetSelect2List(searchInput));
        }

        [AreaConfig(Title = "مشاهده لیست مقادیر تخفیف اظافه", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCarExteraDisocuntValueList([FromQuery]int id)
        {
            return Json(CarExteraDiscountValueManager.GetLightListByCarExteraDiscountId(id));
        }
    }
}
