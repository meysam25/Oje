using Microsoft.AspNetCore.Mvc;
using Oje.AccountManager.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.CarBaseData.Interfaces;
using Oje.Section.CarBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Areas.CarBaseData.Controllers
{
    [Area("CarBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = " پایه خودرو (ادمین)", Icon = "fa-car", Title = "گروه بندی تخفیف اضافه")]
    [CustomeAuthorizeFilter]
    public class CarExteraDiscountCategoryController: Controller
    {
        readonly ICarExteraDiscountCategoryManager CarExteraDiscountCategoryManager = null;
        public CarExteraDiscountCategoryController(ICarExteraDiscountCategoryManager CarExteraDiscountCategoryManager)
        {
            this.CarExteraDiscountCategoryManager = CarExteraDiscountCategoryManager;
        }

        [AreaConfig(Title = "گروه بندی تخفیف اضافه", Icon = "fa-object-group", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "گروه بندی تخفیف اضافه";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "CarExteraDiscountCategory", new { area = "CarBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست گروه بندی تخفیف اضافه", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("CarBaseData", "CarExteraDiscountCategory")));
        }

        [AreaConfig(Title = "افزودن گروه بندی تخفیف اضافه جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateCarExteraDiscountCategoryVM input)
        {
            return Json(CarExteraDiscountCategoryManager.Create(input));
        }

        [AreaConfig(Title = "حذف گروه بندی تخفیف اضافه", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(CarExteraDiscountCategoryManager.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک گروه بندی تخفیف اضافه", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(CarExteraDiscountCategoryManager.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  گروه بندی تخفیف اضافه", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateCarExteraDiscountCategoryVM input)
        {
            return Json(CarExteraDiscountCategoryManager.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست گروه بندی تخفیف اضافه", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] CarExteraDiscountCategoryMainGrid searchInput)
        {
            return Json(CarExteraDiscountCategoryManager.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] CarExteraDiscountCategoryMainGrid searchInput)
        {
            var result = CarExteraDiscountCategoryManager.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
