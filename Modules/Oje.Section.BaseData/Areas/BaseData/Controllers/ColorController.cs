using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.BaseData.Interfaces;
using Oje.Section.BaseData.Models.View;
using System;

namespace Oje.Section.BaseData.Areas.BaseData.Controllers
{
    [Area("BaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "پایه", Icon = "fa-archive", Title = "رنگ")]
    [CustomeAuthorizeFilter]
    public class ColorController: Controller
    {
        readonly IColorService ColorService = null;
        public ColorController(
                IColorService ColorService
            )
        {
            this.ColorService = ColorService;
        }

        [AreaConfig(Title = "رنگ", Icon = "fa-palette", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "رنگ";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "Color", new { area = "BaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست رنگ", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("BaseData", "Color")));
        }

        [AreaConfig(Title = "افزودن رنگ جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] ColorCreateUpdateVM input)
        {
            return Json(ColorService.Create(input));
        }

        [AreaConfig(Title = "حذف رنگ", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(ColorService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک رنگ", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(ColorService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  رنگ", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] ColorCreateUpdateVM input)
        {
            return Json(ColorService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست رنگ", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] ColorMainGrid searchInput)
        {
            return Json(ColorService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ColorMainGrid searchInput)
        {
            var result = ColorService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
