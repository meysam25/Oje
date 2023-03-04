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
    [AreaConfig(ModualTitle = "پایه خودرو", Icon = "fa-car", Title = "برند خودرو")]
    [CustomeAuthorizeFilter]
    public class VehicleSystemController: Controller
    {
        readonly IVehicleSystemService VehicleSystemService = null;
        readonly IVehicleTypeService VehicleTypeService = null;
        public VehicleSystemController
            (
                IVehicleSystemService VehicleSystemService,
                IVehicleTypeService VehicleTypeService
            )
        {
            this.VehicleSystemService = VehicleSystemService;
            this.VehicleTypeService = VehicleTypeService;
        }

        [AreaConfig(Title = "برند خودرو", Icon = "fa-copyright", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "برند خودرو";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "VehicleSystem", new { area = "CarBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست برند خودرو", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("CarBaseData", "VehicleSystem")));
        }

        [AreaConfig(Title = "افزودن برند خودرو جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateVehicleSystemVM input)
        {
            return Json(VehicleSystemService.Create(input));
        }

        [AreaConfig(Title = "حذف برند خودرو", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(VehicleSystemService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک برند خودرو", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(VehicleSystemService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  برند خودرو", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateVehicleSystemVM input)
        {
            return Json(VehicleSystemService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست برند خودرو", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] VehicleSystemMainGrid searchInput)
        {
            return Json(VehicleSystemService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] VehicleSystemMainGrid searchInput)
        {
            var result = VehicleSystemService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست نوع خودرو", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCarTypeList()
        {
            return Json(VehicleTypeService.GetLightList());
        }
    }
}
