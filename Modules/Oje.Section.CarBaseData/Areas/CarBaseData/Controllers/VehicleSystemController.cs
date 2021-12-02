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
    [AreaConfig(ModualTitle = " پایه خودرو (ادمین)", Icon = "fa-car", Title = "برند خودرو")]
    [CustomeAuthorizeFilter]
    public class VehicleSystemController: Controller
    {
        readonly IVehicleSystemManager VehicleSystemManager = null;
        readonly ICarTypeManager CarTypeManager = null;
        public VehicleSystemController(IVehicleSystemManager VehicleSystemManager, ICarTypeManager CarTypeManager)
        {
            this.VehicleSystemManager = VehicleSystemManager;
            this.CarTypeManager = CarTypeManager;
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
            return Json(VehicleSystemManager.Create(input));
        }

        [AreaConfig(Title = "حذف برند خودرو", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(VehicleSystemManager.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک برند خودرو", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(VehicleSystemManager.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  برند خودرو", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateVehicleSystemVM input)
        {
            return Json(VehicleSystemManager.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست برند خودرو", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] VehicleSystemMainGrid searchInput)
        {
            return Json(VehicleSystemManager.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] VehicleSystemMainGrid searchInput)
        {
            var result = VehicleSystemManager.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "لیست کاربری خودرو", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCarTypeList()
        {
            return Json(CarTypeManager.GetLightList());
        }
    }
}
