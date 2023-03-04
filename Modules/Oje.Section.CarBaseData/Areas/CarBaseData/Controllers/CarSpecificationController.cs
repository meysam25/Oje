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
    [AreaConfig(ModualTitle = "پایه خودرو", Icon = "fa-car", Title = "جزئیات خصوصیات خودرو")]
    [CustomeAuthorizeFilter]
    public class CarSpecificationController : Controller
    {
        readonly ICarSpecificationService CarSpecificationService = null;
        readonly IVehicleSpecCategoryService VehicleSpecCategoryService = null;
        readonly IVehicleSystemService VehicleSystemService = null;
        readonly IVehicleSpecService VehicleSpecService = null;
        readonly IVehicleTypeService VehicleTypeService = null;
        public CarSpecificationController
            (
                ICarSpecificationService CarSpecificationService,
                IVehicleSpecCategoryService VehicleSpecCategoryService,
                IVehicleSystemService VehicleSystemService,
                IVehicleSpecService VehicleSpecService,
                IVehicleTypeService VehicleTypeService
            )
        {
            this.CarSpecificationService = CarSpecificationService;
            this.VehicleSpecCategoryService = VehicleSpecCategoryService;
            this.VehicleSystemService = VehicleSystemService;
            this.VehicleSpecService = VehicleSpecService;
            this.VehicleTypeService = VehicleTypeService;
        }

        [AreaConfig(Title = "جزئیات خصوصیات خودرو", Icon = "fa-truck-loading", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "جزئیات خصوصیات خودرو";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "CarSpecification", new { area = "CarBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست جزئیات خصوصیات خودرو", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("CarBaseData", "CarSpecification")));
        }

        [AreaConfig(Title = "افزودن جزئیات خصوصیات خودرو جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateCarSpecificationVM input)
        {
            return Json(CarSpecificationService.Create(input));
        }

        [AreaConfig(Title = "حذف جزئیات خصوصیات خودرو", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(CarSpecificationService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک جزئیات خصوصیات خودرو", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(CarSpecificationService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  جزئیات خصوصیات خودرو", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateCarSpecificationVM input)
        {
            return Json(CarSpecificationService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست جزئیات خصوصیات خودرو", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] CarSpecificationMainGrid searchInput)
        {
            return Json(CarSpecificationService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] CarSpecificationMainGrid searchInput)
        {
            var result = CarSpecificationService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست گروه بندی خصوصیات خودرو", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCarSpecCategoryList()
        {
            return Json(VehicleSpecCategoryService.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست نوع خودرو", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetVehicleTypeList()
        {
            return Json(VehicleTypeService.GetLightList());
        }


        [AreaConfig(Title = "مشاهده لیست برند خودرو", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetBrandList([FromQuery] Select2SearchVM searchInput, [FromQuery] int? vtId)
        {
            return Json(VehicleSystemService.GetSelect2List(searchInput, vtId));
        }

        [AreaConfig(Title = "مشاهده لیست خصوصیات خودرو", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetVehicleSpecList([FromQuery] Select2SearchVM searchInput, [FromQuery] int? catId, [FromQuery] int? sysId, [FromQuery] int? vtId, [FromQuery] bool? isDeter)
        {
            return Json(VehicleSpecService.GetLightList(catId, sysId, searchInput, vtId, isDeter));
        }
    }
}
