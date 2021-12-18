using Oje.AccountService.Filters;
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
    [AreaConfig(ModualTitle = " پایه خودرو (ادمین)", Icon = "fa-car", Title = "کاربری خودرو")]
    [CustomeAuthorizeFilter]
    public class CarTypeController: Controller
    {
        readonly ICarTypeService CarTypeService = null;
        public CarTypeController(ICarTypeService CarTypeService)
        {
            this.CarTypeService = CarTypeService;
        }

        [AreaConfig(Title = "کاربری خودرو", Icon = "fa-truck-moving", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "کاربری خودرو";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "CarType", new { area = "CarBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست کاربری خودرو", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("CarBaseData", "CarType")));
        }

        [AreaConfig(Title = "افزودن کاربری خودرو جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateCarTypeVM input)
        {
            return Json(CarTypeService.Create(input));
        }

        [AreaConfig(Title = "حذف کاربری خودرو", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(CarTypeService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک کاربری خودرو", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(CarTypeService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  کاربری خودرو", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateCarTypeVM input)
        {
            return Json(CarTypeService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست کاربری خودرو", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] CarTypeMainGrid searchInput)
        {
            return Json(CarTypeService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] CarTypeMainGrid searchInput)
        {
            var result = CarTypeService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
