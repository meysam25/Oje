using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.CarThirdBaseData.Interfaces;
using Oje.Section.CarThirdBaseData.Models.View;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Oje.Section.CarThirdBaseData.Areas.CarThirdBaseData.Controllers
{
    [Area("CarThirdBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = " پایه استعلام ثالث خودرو (ادمین)", Icon = "fa-car-side", Title = "درصد عدم خسارت جانی")]
    [CustomeAuthorizeFilter]
    public class ThirdPartyBodyNoDamageDiscountHistoryController: Controller
    {
        readonly IThirdPartyBodyNoDamageDiscountHistoryService ThirdPartyBodyNoDamageDiscountHistoryService = null;
        public ThirdPartyBodyNoDamageDiscountHistoryController
            (
                IThirdPartyBodyNoDamageDiscountHistoryService ThirdPartyBodyNoDamageDiscountHistoryService
            )
        {
            this.ThirdPartyBodyNoDamageDiscountHistoryService = ThirdPartyBodyNoDamageDiscountHistoryService;
        }

        [AreaConfig(Title = "درصد عدم خسارت جانی", Icon = "fa-percent", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "درصد عدم خسارت جانی";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ThirdPartyBodyNoDamageDiscountHistory", new { area = "CarThirdBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست درصد عدم خسارت جانی", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("CarThirdBaseData", "ThirdPartyBodyNoDamageDiscountHistory")));
        }

        [AreaConfig(Title = "افزودن درصد عدم خسارت جانی جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateThirdPartyBodyNoDamageDiscountHistoryVM input)
        {
            return Json(ThirdPartyBodyNoDamageDiscountHistoryService.Create(input));
        }

        [AreaConfig(Title = "حذف درصد عدم خسارت جانی", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(ThirdPartyBodyNoDamageDiscountHistoryService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک درصد عدم خسارت جانی", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(ThirdPartyBodyNoDamageDiscountHistoryService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  درصد عدم خسارت جانی", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateThirdPartyBodyNoDamageDiscountHistoryVM input)
        {
            return Json(ThirdPartyBodyNoDamageDiscountHistoryService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست درصد عدم خسارت جانی", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] ThirdPartyBodyNoDamageDiscountHistoryMainGrid searchInput)
        {
            return Json(ThirdPartyBodyNoDamageDiscountHistoryService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ThirdPartyBodyNoDamageDiscountHistoryMainGrid searchInput)
        {
            var result = ThirdPartyBodyNoDamageDiscountHistoryService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
