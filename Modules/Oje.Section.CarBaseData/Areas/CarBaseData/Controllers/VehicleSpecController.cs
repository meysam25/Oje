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
    [AreaConfig(ModualTitle = "پایه خودرو", Icon = "fa-car", Title = "خصوصیات خودرو")]
    [CustomeAuthorizeFilter]
    public class VehicleSpecController: Controller
    {
        readonly IVehicleSpecService VehicleSpecService = null;
        readonly IVehicleSpecCategoryService VehicleSpecCategoryService = null;
        readonly IVehicleSystemService VehicleSystemService = null;
        readonly IVehicleTypeService VehicleTypeService = null;
        public VehicleSpecController(
                IVehicleSpecService VehicleSpecService, 
                IVehicleSpecCategoryService VehicleSpecCategoryService,
                IVehicleSystemService VehicleSystemService,
                IVehicleTypeService VehicleTypeService
            )
        {
            this.VehicleSpecService = VehicleSpecService;
            this.VehicleSpecCategoryService = VehicleSpecCategoryService;
            this.VehicleSystemService = VehicleSystemService;
            this.VehicleTypeService = VehicleTypeService;
        }

        [AreaConfig(Title = "خصوصیات خودرو", Icon = "fa-car-battery", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "خصوصیات خودرو";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "VehicleSpec", new { area = "CarBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست خصوصیات خودرو", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("CarBaseData", "VehicleSpec")));
        }

        [AreaConfig(Title = "افزودن خصوصیات خودرو جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] VehicleSpecCreateUpdateVM input)
        {
            return Json(VehicleSpecService.Create(input));
        }

        [AreaConfig(Title = "حذف خصوصیات خودرو", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(VehicleSpecService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک خصوصیات خودرو", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(VehicleSpecService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  خصوصیات خودرو", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] VehicleSpecCreateUpdateVM input)
        {
            return Json(VehicleSpecService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست خصوصیات خودرو", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] VehicleSpecMainGrid searchInput)
        {
            return Json(VehicleSpecService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] VehicleSpecMainGrid searchInput)
        {
            var result = VehicleSpecService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "لیست گروه بندی", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCategoryList()
        {
            return Json(VehicleSpecCategoryService.GetLightList());
        }

        [AreaConfig(Title = "لیست نوع خودرو", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetVehicleTypeList()
        {
            return Json(VehicleTypeService.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست برند خودرو", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetVSystemList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(VehicleSystemService.GetSelect2List(searchInput, null));
        }
    }
}
