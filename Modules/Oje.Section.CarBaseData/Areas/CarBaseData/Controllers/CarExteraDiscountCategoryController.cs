using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.CarBaseData.Interfaces;
using Oje.Section.CarBaseData.Models.View;
using System;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.CarBaseData.Areas.CarBaseData.Controllers
{
    [Area("CarBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "پایه خودرو", Icon = "fa-car", Title = "گروه بندی تخفیف اضافه")]
    [CustomeAuthorizeFilter]
    public class CarExteraDiscountCategoryController: Controller
    {
        readonly ICarExteraDiscountCategoryService CarExteraDiscountCategoryService = null;
        public CarExteraDiscountCategoryController(ICarExteraDiscountCategoryService CarExteraDiscountCategoryService)
        {
            this.CarExteraDiscountCategoryService = CarExteraDiscountCategoryService;
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
            return Json(CarExteraDiscountCategoryService.Create(input));
        }

        [AreaConfig(Title = "حذف گروه بندی تخفیف اضافه", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(CarExteraDiscountCategoryService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک گروه بندی تخفیف اضافه", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(CarExteraDiscountCategoryService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  گروه بندی تخفیف اضافه", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateCarExteraDiscountCategoryVM input)
        {
            return Json(CarExteraDiscountCategoryService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست گروه بندی تخفیف اضافه", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] CarExteraDiscountCategoryMainGrid searchInput)
        {
            return Json(CarExteraDiscountCategoryService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] CarExteraDiscountCategoryMainGrid searchInput)
        {
            var result = CarExteraDiscountCategoryService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
