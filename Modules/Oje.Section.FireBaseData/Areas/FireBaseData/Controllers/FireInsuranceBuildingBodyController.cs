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
    [AreaConfig(ModualTitle = " پایه استعلام آتش سوزی (ادمین)", Icon = "fa-fire", Title = "اسکلت ساختمان")]
    [CustomeAuthorizeFilter]
    public class FireInsuranceBuildingBodyController: Controller
    {
        readonly IFireInsuranceBuildingBodyService FireInsuranceBuildingBodyService = null;
        public FireInsuranceBuildingBodyController(IFireInsuranceBuildingBodyService FireInsuranceBuildingBodyService)
        {
            this.FireInsuranceBuildingBodyService = FireInsuranceBuildingBodyService;
        }

        [AreaConfig(Title = "اسکلت ساختمان", Icon = "fa-building", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "اسکلت ساختمان";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "FireInsuranceBuildingBody", new { area = "FireBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست اسکلت ساختمان", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("FireBaseData", "FireInsuranceBuildingBody")));
        }

        [AreaConfig(Title = "افزودن اسکلت ساختمان جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateFireInsuranceBuildingBodyVM input)
        {
            return Json(FireInsuranceBuildingBodyService.Create(input));
        }

        [AreaConfig(Title = "حذف اسکلت ساختمان", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(FireInsuranceBuildingBodyService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک اسکلت ساختمان", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(FireInsuranceBuildingBodyService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  اسکلت ساختمان", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateFireInsuranceBuildingBodyVM input)
        {
            return Json(FireInsuranceBuildingBodyService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست اسکلت ساختمان", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] FireInsuranceBuildingBodyMainGrid searchInput)
        {
            return Json(FireInsuranceBuildingBodyService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] FireInsuranceBuildingBodyMainGrid searchInput)
        {
            var result = FireInsuranceBuildingBodyService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
