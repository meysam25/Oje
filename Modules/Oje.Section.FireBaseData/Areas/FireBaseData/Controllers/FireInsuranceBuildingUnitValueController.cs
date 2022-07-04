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
    [AreaConfig(ModualTitle = " پایه استعلام آتش سوزی (ادمین)", Icon = "fa-fire", Title = "ارزش ساخت هر متر مربع")]
    [CustomeAuthorizeFilter]
    public class FireInsuranceBuildingUnitValueController: Controller
    {
        readonly IFireInsuranceBuildingUnitValueService FireInsuranceBuildingUnitValueService = null;
        public FireInsuranceBuildingUnitValueController(IFireInsuranceBuildingUnitValueService FireInsuranceBuildingUnitValueService)
        {
            this.FireInsuranceBuildingUnitValueService = FireInsuranceBuildingUnitValueService;
        }

        [AreaConfig(Title = "ارزش ساخت هر متر مربع", Icon = "fa-dollar-sign", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "ارزش ساخت هر متر مربع";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "FireInsuranceBuildingUnitValue", new { area = "FireBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست ارزش ساخت هر متر مربع", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("FireBaseData", "FireInsuranceBuildingUnitValue")));
        }

        [AreaConfig(Title = "افزودن ارزش ساخت هر متر مربع جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateFireInsuranceBuildingUnitValueVM input)
        {
            return Json(FireInsuranceBuildingUnitValueService.Create(input));
        }

        [AreaConfig(Title = "حذف ارزش ساخت هر متر مربع", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(FireInsuranceBuildingUnitValueService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک ارزش ساخت هر متر مربع", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(FireInsuranceBuildingUnitValueService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  ارزش ساخت هر متر مربع", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateFireInsuranceBuildingUnitValueVM input)
        {
            return Json(FireInsuranceBuildingUnitValueService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست ارزش ساخت هر متر مربع", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] FireInsuranceBuildingUnitValueMainGrid searchInput)
        {
            return Json(FireInsuranceBuildingUnitValueService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] FireInsuranceBuildingUnitValueMainGrid searchInput)
        {
            var result = FireInsuranceBuildingUnitValueService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
