using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Sanab.Interfaces;
using Oje.Sanab.Models.View;
using System;

namespace Oje.Section.Sanab.Areas.SanabAdmin.Controllers
{
    [Area("SanabAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "سنحاب", Icon = "fa-cctv", Title = "نوع خودرو")]
    [CustomeAuthorizeFilter]
    public class SanabVehicleTypeController: Controller
    {
        readonly ISanabVehicleTypeService SanabVehicleTypeService = null;
        readonly IVehicleTypeService VehicleTypeService = null;

        public SanabVehicleTypeController
            (
                ISanabVehicleTypeService SanabVehicleTypeService,
                IVehicleTypeService VehicleTypeService
            )
        {
            this.SanabVehicleTypeService = SanabVehicleTypeService;
            this.VehicleTypeService = VehicleTypeService;
        }

        [AreaConfig(Title = "نوع خودرو", Icon = "fa-trailer", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "نوع خودرو";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "SanabVehicleType", new { area = "SanabAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست نوع خودرو", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("SanabAdmin", "SanabVehicleType")));
        }

        [AreaConfig(Title = "افزودن نوع خودرو جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] SanabVehicleTypeCreateUpdateVM input)
        {
            return Json(SanabVehicleTypeService.Create(input));
        }

        [AreaConfig(Title = "حذف نوع خودرو", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(SanabVehicleTypeService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک نوع خودرو", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(SanabVehicleTypeService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  نوع خودرو", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] SanabVehicleTypeCreateUpdateVM input)
        {
            return Json(SanabVehicleTypeService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست نوع خودرو", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] SanabVehicleTypeMainGrid searchInput)
        {
            return Json(SanabVehicleTypeService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] SanabVehicleTypeMainGrid searchInput)
        {
            var result = SanabVehicleTypeService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست نوع خودرو", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetVTList()
        {
            return Json(VehicleTypeService.GetLightList());
        }
    }
}
