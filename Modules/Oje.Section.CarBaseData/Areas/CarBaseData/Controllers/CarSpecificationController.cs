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
    [AreaConfig(ModualTitle = " پایه خودرو (ادمین)", Icon = "fa-car", Title = "خصوصیات خودرو")]
    [CustomeAuthorizeFilter]
    public class CarSpecificationController: Controller
    {
        readonly ICarSpecificationManager CarSpecificationManager = null;
        public CarSpecificationController(ICarSpecificationManager CarSpecificationManager)
        {
            this.CarSpecificationManager = CarSpecificationManager;
        }

        [AreaConfig(Title = "خصوصیات خودرو", Icon = "fa-truck-loading", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "خصوصیات خودرو";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "CarSpecification", new { area = "CarBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست خصوصیات خودرو", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("CarBaseData", "CarSpecification")));
        }

        [AreaConfig(Title = "افزودن خصوصیات خودرو جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateCarSpecificationVM input)
        {
            return Json(CarSpecificationManager.Create(input));
        }

        [AreaConfig(Title = "حذف خصوصیات خودرو", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(CarSpecificationManager.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک خصوصیات خودرو", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(CarSpecificationManager.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  خصوصیات خودرو", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateCarSpecificationVM input)
        {
            return Json(CarSpecificationManager.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست خصوصیات خودرو", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] CarSpecificationMainGrid searchInput)
        {
            return Json(CarSpecificationManager.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] CarSpecificationMainGrid searchInput)
        {
            var result = CarSpecificationManager.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
