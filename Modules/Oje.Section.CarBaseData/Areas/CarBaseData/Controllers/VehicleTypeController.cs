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
    [AreaConfig(ModualTitle = " پایه خودرو (ادمین)", Icon = "fa-car", Title = "نوع خودرو")]
    [CustomeAuthorizeFilter]
    public class VehicleTypeController: Controller
    {
        readonly IVehicleTypeManager VehicleTypeManager = null;
        readonly ICarSpecificationManager CarSpecificationManager = null;
        readonly IVehicleSystemManager VehicleSystemManager = null;
        public VehicleTypeController(IVehicleTypeManager VehicleTypeManager, ICarSpecificationManager CarSpecificationManager, IVehicleSystemManager VehicleSystemManager)
        {
            this.VehicleTypeManager = VehicleTypeManager;
            this.CarSpecificationManager = CarSpecificationManager;
            this.VehicleSystemManager = VehicleSystemManager;
        }

        [AreaConfig(Title = "نوع خودرو", Icon = "fa-trailer", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "نوع خودرو";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "VehicleType", new { area = "CarBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست نوع خودرو", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("CarBaseData", "VehicleType")));
        }

        [AreaConfig(Title = "افزودن نوع خودرو جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateVehicleTypeVM input)
        {
            return Json(VehicleTypeManager.Create(input));
        }

        [AreaConfig(Title = "حذف نوع خودرو", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(VehicleTypeManager.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک نوع خودرو", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(VehicleTypeManager.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  نوع خودرو", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateVehicleTypeVM input)
        {
            return Json(VehicleTypeManager.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست نوع خودرو", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] VehicleTypeMainGrid searchInput)
        {
            return Json(VehicleTypeManager.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] VehicleTypeMainGrid searchInput)
        {
            var result = VehicleTypeManager.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست خصوصیات خودرو", Icon = "fa-list-alt ")]
        [HttpGet]
        public ActionResult GetCarSpecList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(CarSpecificationManager.GetSelect2List(searchInput));
        }

        [AreaConfig(Title = "مشاهده لیست برند خودرو", Icon = "fa-list-alt ")]
        [HttpGet]
        public ActionResult GetCarBrandList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(VehicleSystemManager.GetSelect2List(searchInput));
        }
    }
}
