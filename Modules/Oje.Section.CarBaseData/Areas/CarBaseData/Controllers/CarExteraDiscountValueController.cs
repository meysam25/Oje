using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.CarBaseData.Interfaces;
using Oje.Section.CarBaseData.Models.View;
using Microsoft.AspNetCore.Mvc;
using System;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.CarBaseData.Areas.CarBaseData.Controllers
{
    [Area("CarBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "پایه خودرو", Icon = "fa-car", Title = "مقادیر تخفیف اضافه")]
    [CustomeAuthorizeFilter]
    public class CarExteraDiscountValueController: Controller
    {
        readonly ICarExteraDiscountValueService CarExteraDiscountValueService = null;
        readonly ICarExteraDiscountService CarExteraDiscountService = null;
        public CarExteraDiscountValueController(ICarExteraDiscountValueService CarExteraDiscountValueService, ICarExteraDiscountService CarExteraDiscountService)
        {
            this.CarExteraDiscountValueService = CarExteraDiscountValueService;
            this.CarExteraDiscountService = CarExteraDiscountService;
        }

        [AreaConfig(Title = "مقادیر تخفیف اضافه", Icon = "fa-check-circle", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "مقادیر تخفیف اضافه";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "CarExteraDiscountValue", new { area = "CarBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست مقادیر تخفیف اضافه", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("CarBaseData", "CarExteraDiscountValue")));
        }

        [AreaConfig(Title = "افزودن مقادیر تخفیف اضافه جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateCarExteraDiscountValueVM input)
        {
            return Json(CarExteraDiscountValueService.Create(input));
        }

        [AreaConfig(Title = "حذف مقادیر تخفیف اضافه", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(CarExteraDiscountValueService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک مقادیر تخفیف اضافه", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(CarExteraDiscountValueService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  مقادیر تخفیف اضافه", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateCarExteraDiscountValueVM input)
        {
            return Json(CarExteraDiscountValueService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست مقادیر تخفیف اضافه", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] CarExteraDiscountValueMainGrid searchInput)
        {
            return Json(CarExteraDiscountValueService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] CarExteraDiscountValueMainGrid searchInput)
        {
            var result = CarExteraDiscountValueService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }


        [AreaConfig(Title = "مشاهده لیست تخفیف اظافه", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetCarExteraDiscountList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(CarExteraDiscountService.GetSelect2List(searchInput));
        }
    }
}
