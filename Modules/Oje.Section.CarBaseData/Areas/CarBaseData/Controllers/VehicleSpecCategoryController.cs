using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
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
    [AreaConfig(ModualTitle = " پایه خودرو (ادمین)", Icon = "fa-car", Title = "گروه بندی خصوصیات خودرو")]
    [CustomeAuthorizeFilter]
    public class VehicleSpecCategoryController: Controller
    {
        readonly IVehicleSpecCategoryService VehicleSpecCategoryService = null;
        public VehicleSpecCategoryController(
                IVehicleSpecCategoryService VehicleSpecCategoryService
            )
        {
            this.VehicleSpecCategoryService = VehicleSpecCategoryService;
        }

        [AreaConfig(Title = "گروه بندی خصوصیت خودرو", Icon = "fa-object-group", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "گروه بندی خصوصیت خودرو";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "VehicleSpecCategory", new { area = "CarBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست گروه بندی خصوصیت خودرو", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("CarBaseData", "VehicleSpecCategory")));
        }

        [AreaConfig(Title = "افزودن گروه بندی خصوصیت خودرو جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] VehicleSpecCategoryCreateUpdateVM input)
        {
            return Json(VehicleSpecCategoryService.Create(input));
        }

        [AreaConfig(Title = "حذف گروه بندی خصوصیت خودرو", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(VehicleSpecCategoryService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک گروه بندی خصوصیت خودرو", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(VehicleSpecCategoryService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  گروه بندی خصوصیت خودرو", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] VehicleSpecCategoryCreateUpdateVM input)
        {
            return Json(VehicleSpecCategoryService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست گروه بندی خصوصیت خودرو", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] VehicleSpecCategoryMainGrid searchInput)
        {
            return Json(VehicleSpecCategoryService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] VehicleSpecCategoryMainGrid searchInput)
        {
            var result = VehicleSpecCategoryService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
