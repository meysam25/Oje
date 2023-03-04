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
    [AreaConfig(ModualTitle = "پایه خودرو", Icon = "fa-car", Title = "کاربری خودرو محاسباتی")]
    [CustomeAuthorizeFilter]
    public class VehicleUsageController: Controller
    {
        readonly IVehicleUsageService VehicleUsageService = null;
        readonly ICarTypeService CarTypeService = null;
        public VehicleUsageController(IVehicleUsageService VehicleUsageService, ICarTypeService CarTypeService)
        {
            this.VehicleUsageService = VehicleUsageService;
            this.CarTypeService = CarTypeService;
        }

        [AreaConfig(Title = "کاربری خودرو محاسباتی", Icon = "fa-money-check-alt", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "کاربری خودرو محاسباتی";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "VehicleUsage", new { area = "CarBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست کاربری خودرو محاسباتی", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("CarBaseData", "VehicleUsage")));
        }

        [AreaConfig(Title = "افزودن کاربری خودرو محاسباتی جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateVehicleUsageVM input)
        {
            return Json(VehicleUsageService.Create(input));
        }

        [AreaConfig(Title = "حذف کاربری خودرو محاسباتی", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(VehicleUsageService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک کاربری خودرو محاسباتی", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(VehicleUsageService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  کاربری خودرو محاسباتی", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateVehicleUsageVM input)
        {
            return Json(VehicleUsageService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست کاربری خودرو محاسباتی", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] VehicleUsageMainGrid searchInput)
        {
            return Json(VehicleUsageService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] VehicleUsageMainGrid searchInput)
        {
            var result = VehicleUsageService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست کاربری خودرو", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetCarTypeList()
        {
            return Json(CarTypeService.GetLightList());
        }
    }
}
