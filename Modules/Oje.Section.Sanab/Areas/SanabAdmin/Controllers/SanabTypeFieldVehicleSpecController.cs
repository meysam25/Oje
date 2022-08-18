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
    [AreaConfig(ModualTitle = "سنحاب", Icon = "fa-cctv", Title = "خصوصیت خودرو")]
    [CustomeAuthorizeFilter]
    public class SanabTypeFieldVehicleSpecController: Controller
    {
        readonly ISanabTypeFieldVehicleSpecService SanabTypeFieldVehicleSpecService = null;
        readonly IVehicleSystemService VehicleSystemService = null;
        readonly IVehicleSpecService VehicleSpecService = null;

        public SanabTypeFieldVehicleSpecController
            (
                ISanabTypeFieldVehicleSpecService SanabTypeFieldVehicleSpecService,
                IVehicleSystemService VehicleSystemService,
                IVehicleSpecService VehicleSpecService
            )
        {
            this.SanabTypeFieldVehicleSpecService = SanabTypeFieldVehicleSpecService;
            this.VehicleSystemService = VehicleSystemService;
            this.VehicleSpecService = VehicleSpecService;
        }

        [AreaConfig(Title = "خصوصیت خودرو", Icon = "fa-car-bus", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "خصوصیت خودرو";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "SanabTypeFieldVehicleSpec", new { area = "SanabAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست خصوصیت خودرو", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("SanabAdmin", "SanabTypeFieldVehicleSpec")));
        }

        [AreaConfig(Title = "افزودن خصوصیت خودرو جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] SanabTypeFieldVehicleSpecCreateUpdateVM input)
        {
            return Json(SanabTypeFieldVehicleSpecService.Create(input));
        }

        [AreaConfig(Title = "حذف خصوصیت خودرو", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(SanabTypeFieldVehicleSpecService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک خصوصیت خودرو", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(SanabTypeFieldVehicleSpecService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  خصوصیت خودرو", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] SanabTypeFieldVehicleSpecCreateUpdateVM input)
        {
            return Json(SanabTypeFieldVehicleSpecService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست خصوصیت خودرو", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] SanabTypeFieldVehicleSpecMainGrid searchInput)
        {
            return Json(SanabTypeFieldVehicleSpecService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] SanabTypeFieldVehicleSpecMainGrid searchInput)
        {
            var result = SanabTypeFieldVehicleSpecService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست خصوصیت خودرو", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetVSystemList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(VehicleSystemService.GetSelect2List(searchInput));
        }

        [AreaConfig(Title = "مشاهده لیست خصوصیت خودرو", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetVSpecList([FromQuery] Select2SearchVM searchInput, [FromQuery] int? vId)
        {
            return Json(VehicleSpecService.GetSelect2List(searchInput, vId));
        }
    }
}
