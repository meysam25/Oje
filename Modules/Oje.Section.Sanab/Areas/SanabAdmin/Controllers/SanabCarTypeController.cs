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
    [AreaConfig(ModualTitle = "سنحاب", Icon = "fa-cctv", Title = "کاربری خودرو")]
    [CustomeAuthorizeFilter]
    public class SanabCarTypeController: Controller
    {
        readonly ISanabCarTypeService SanabCarTypeService = null;
        readonly ICarTypeService CarTypeService = null;

        public SanabCarTypeController
            (
                ISanabCarTypeService SanabCarTypeService,
                ICarTypeService CarTypeService
            )
        {
            this.SanabCarTypeService = SanabCarTypeService;
            this.CarTypeService = CarTypeService;
        }

        [AreaConfig(Title = "کاربری خودرو", Icon = "fa-truck-moving", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "کاربری خودرو";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "SanabCarType", new { area = "SanabAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست کاربری خودرو", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("SanabAdmin", "SanabCarType")));
        }

        [AreaConfig(Title = "افزودن کاربری خودرو جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] SanabCarTypeCreateUpdateVM input)
        {
            return Json(SanabCarTypeService.Create(input));
        }

        [AreaConfig(Title = "حذف کاربری خودرو", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(SanabCarTypeService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک کاربری خودرو", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(SanabCarTypeService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  کاربری خودرو", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] SanabCarTypeCreateUpdateVM input)
        {
            return Json(SanabCarTypeService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست کاربری خودرو", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] SanabCarTypeMainGrid searchInput)
        {
            return Json(SanabCarTypeService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] SanabCarTypeMainGrid searchInput)
        {
            var result = SanabCarTypeService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست کاربری خودرو", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCarTypeList()
        {
            return Json(CarTypeService.GetLightList());
        }
    }
}
