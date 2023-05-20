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
    [AreaConfig(ModualTitle = "سنحاب", Icon = "fa-cctv", Title = "برند خودرو")]
    [CustomeAuthorizeFilter]
    public class SanabSystemFieldVehicleSystemController: Controller
    {
        readonly ISanabSystemFieldVehicleSystemService SanabSystemFieldVehicleSystemService = null;
        readonly IVehicleSystemService VehicleSystemService = null;

        public SanabSystemFieldVehicleSystemController
            (
                ISanabSystemFieldVehicleSystemService SanabSystemFieldVehicleSystemService,
                IVehicleSystemService VehicleSystemService
            )
        {
            this.SanabSystemFieldVehicleSystemService = SanabSystemFieldVehicleSystemService;
            this.VehicleSystemService = VehicleSystemService;
        }

        [AreaConfig(Title = "برند خودرو", Icon = "fa-copyright", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "برند خودرو";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "SanabSystemFieldVehicleSystem", new { area = "SanabAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست برند خودرو", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("SanabAdmin", "SanabSystemFieldVehicleSystem")));
        }

        [AreaConfig(Title = "افزودن برند خودرو جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] SanabSystemFieldVehicleSystemCreateUpdateVM input)
        {
            return Json(SanabSystemFieldVehicleSystemService.Create(input));
        }

        [AreaConfig(Title = "حذف برند خودرو", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(SanabSystemFieldVehicleSystemService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک برند خودرو", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(SanabSystemFieldVehicleSystemService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  برند خودرو", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] SanabSystemFieldVehicleSystemCreateUpdateVM input)
        {
            return Json(SanabSystemFieldVehicleSystemService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست برند خودرو", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] SanabSystemFieldVehicleSystemMainGrid searchInput)
        {
            return Json(SanabSystemFieldVehicleSystemService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] SanabSystemFieldVehicleSystemMainGrid searchInput)
        {
            var result = SanabSystemFieldVehicleSystemService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست برند خودرو", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetVSystemList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(VehicleSystemService.GetSelect2List(searchInput));
        }
    }
}
