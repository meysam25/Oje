using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.CarBodyBaseData.Interfaces;
using Oje.Section.CarBodyBaseData.Models.View;
using System;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.CarBodyBaseData.Areas.CarBodyBaseData.Controllers
{
    [Area("CarBodyBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = " پایه استعلام بدنه خودرو (ادمین)", Icon = "fa-car-crash", Title = "درصد حق بیمه سال ساخت بدنه")]
    [CustomeAuthorizeFilter]
    public class CarBodyCreateDatePercentController: Controller
    {
        readonly ICarBodyCreateDatePercentService CarBodyCreateDatePercentService = null;
        public CarBodyCreateDatePercentController(ICarBodyCreateDatePercentService CarBodyCreateDatePercentService)
        {
            this.CarBodyCreateDatePercentService = CarBodyCreateDatePercentService;
        }

        [AreaConfig(Title = "درصد حق بیمه سال ساخت بدنه", Icon = "fa-comments-dollar", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "درصد حق بیمه سال ساخت بدنه";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "CarBodyCreateDatePercent", new { area = "CarBodyBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست درصد حق بیمه سال ساخت بدنه", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("CarBodyBaseData", "CarBodyCreateDatePercent")));
        }

        [AreaConfig(Title = "افزودن درصد حق بیمه سال ساخت بدنه جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateCarBodyCreateDatePercentVM input)
        {
            return Json(CarBodyCreateDatePercentService.Create(input));
        }

        [AreaConfig(Title = "حذف درصد حق بیمه سال ساخت بدنه", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(CarBodyCreateDatePercentService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک درصد حق بیمه سال ساخت بدنه", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(CarBodyCreateDatePercentService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  درصد حق بیمه سال ساخت بدنه", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateCarBodyCreateDatePercentVM input)
        {
            return Json(CarBodyCreateDatePercentService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست درصد حق بیمه سال ساخت بدنه", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] CarBodyCreateDatePercentMainGrid searchInput)
        {
            return Json(CarBodyCreateDatePercentService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] CarBodyCreateDatePercentMainGrid searchInput)
        {
            var result = CarBodyCreateDatePercentService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
