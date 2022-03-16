using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.AccountService.Models.View;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using System;

namespace Oje.Section.Account.Areas.Account.Controllers
{
    [Area("Account")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = " هویتی (ادمین)", Icon = "fa-users", Title = "گروه بندی کنترولر منو")]
    [CustomeAuthorizeFilter]
    public class SubSectionCategoryController: Controller
    {
        readonly IControllerCategoryService ControllerCategoryService = null;
        readonly IControllerService ControllerService = null;
        public SubSectionCategoryController
            (
                IControllerCategoryService ControllerCategoryService,
                IControllerService ControllerService
            )
        {
            this.ControllerCategoryService = ControllerCategoryService;
            this.ControllerService = ControllerService;
        }

        [AreaConfig(Title = "گروه بندی کنترولر منو", Icon = "fa-user", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "گروه بندی کنترولر منو";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "SubSectionCategory", new { area = "Account" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست گروه بندی کنترولر منو", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Account", "SubSectionCategory")));
        }

        [AreaConfig(Title = "افزودن گروه بندی کنترولر منو", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] ControllerCategoryCreateUpdateVM input)
        {
            return Json(ControllerCategoryService.Create(input));
        }

        [AreaConfig(Title = "حذف گروه بندی کنترولر منو", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(ControllerCategoryService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده گروه بندی کنترولر منو", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(ControllerCategoryService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی گروه بندی کنترولر منو", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] ControllerCategoryCreateUpdateVM input)
        {
            return Json(ControllerCategoryService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست گروه بندی کنترولر منو", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] ControllerCategoryMainGrid searchInput)
        {
            return Json(ControllerCategoryService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ControllerCategoryMainGrid searchInput)
        {
            var result = ControllerCategoryService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }

        [HttpGet]
        [AreaConfig(Title = "لیست کنترولر ها", Icon = "fa-list-alt")]
        public IActionResult GetControllerList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(ControllerService.GetSelect2List(searchInput));
        }
    }
}
