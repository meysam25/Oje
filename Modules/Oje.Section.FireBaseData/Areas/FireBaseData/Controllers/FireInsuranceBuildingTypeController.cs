using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.FireBaseData.Interfaces;
using Oje.Section.FireBaseData.Models.View;
using Microsoft.AspNetCore.Mvc;
using System;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.FireBaseData.Areas.FireBaseData.Controllers
{
    [Area("FireBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = " پایه استعلام آتش سوزی", Icon = "fa-fire", Title = "نوع ساختمان")]
    [CustomeAuthorizeFilter]
    public class FireInsuranceBuildingTypeController: Controller
    {
        readonly IFireInsuranceBuildingTypeService FireInsuranceBuildingTypeService = null;
        public FireInsuranceBuildingTypeController(
                IFireInsuranceBuildingTypeService FireInsuranceBuildingTypeService
            )
        {
            this.FireInsuranceBuildingTypeService = FireInsuranceBuildingTypeService;
        }

        [AreaConfig(Title = "نوع ساختمان", Icon = "fa-building", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "نوع ساختمان";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "FireInsuranceBuildingType", new { area = "FireBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست نوع ساختمان", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("FireBaseData", "FireInsuranceBuildingType")));
        }

        [AreaConfig(Title = "افزودن نوع ساختمان جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateFireInsuranceBuildingTypeVM input)
        {
            return Json(FireInsuranceBuildingTypeService.Create(input));
        }

        [AreaConfig(Title = "حذف نوع ساختمان", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(FireInsuranceBuildingTypeService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک نوع ساختمان", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(FireInsuranceBuildingTypeService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  نوع ساختمان", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateFireInsuranceBuildingTypeVM input)
        {
            return Json(FireInsuranceBuildingTypeService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست نوع ساختمان", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] FireInsuranceBuildingTypeMainGrid searchInput)
        {
            return Json(FireInsuranceBuildingTypeService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] FireInsuranceBuildingTypeMainGrid searchInput)
        {
            var result = FireInsuranceBuildingTypeService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
