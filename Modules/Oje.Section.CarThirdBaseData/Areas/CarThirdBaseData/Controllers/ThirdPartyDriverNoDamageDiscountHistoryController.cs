using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.CarThirdBaseData.Interfaces;
using Oje.Section.CarThirdBaseData.Models.View;
using Microsoft.AspNetCore.Mvc;
using System;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.CarThirdBaseData.Areas.CarThirdBaseData.Controllers
{
    [Area("CarThirdBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = " پایه استعلام ثالث خودرو (ادمین)", Icon = "fa-car-side", Title = "درصد عدم خسارت راننده")]
    [CustomeAuthorizeFilter]
    public class ThirdPartyDriverNoDamageDiscountHistoryController: Controller
    {
        readonly IThirdPartyDriverNoDamageDiscountHistoryService ThirdPartyDriverNoDamageDiscountHistoryService = null;
        public ThirdPartyDriverNoDamageDiscountHistoryController(
                IThirdPartyDriverNoDamageDiscountHistoryService ThirdPartyDriverNoDamageDiscountHistoryService
            )
        {
            this.ThirdPartyDriverNoDamageDiscountHistoryService = ThirdPartyDriverNoDamageDiscountHistoryService;
        }

        [AreaConfig(Title = "درصد عدم خسارت راننده", Icon = "fa-percent", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "درصد عدم خسارت راننده";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ThirdPartyDriverNoDamageDiscountHistory", new { area = "CarThirdBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست درصد عدم خسارت راننده", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("CarThirdBaseData", "ThirdPartyDriverNoDamageDiscountHistory")));
        }

        [AreaConfig(Title = "افزودن درصد عدم خسارت راننده جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateThirdPartyDriverNoDamageDiscountHistoryVM input)
        {
            return Json(ThirdPartyDriverNoDamageDiscountHistoryService.Create(input));
        }

        [AreaConfig(Title = "حذف درصد عدم خسارت راننده", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(ThirdPartyDriverNoDamageDiscountHistoryService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک درصد عدم خسارت راننده", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(ThirdPartyDriverNoDamageDiscountHistoryService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  درصد عدم خسارت راننده", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateThirdPartyDriverNoDamageDiscountHistoryVM input)
        {
            return Json(ThirdPartyDriverNoDamageDiscountHistoryService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست درصد عدم خسارت راننده", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] ThirdPartyDriverNoDamageDiscountHistoryMainGrid searchInput)
        {
            return Json(ThirdPartyDriverNoDamageDiscountHistoryService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ThirdPartyDriverNoDamageDiscountHistoryMainGrid searchInput)
        {
            var result = ThirdPartyDriverNoDamageDiscountHistoryService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
