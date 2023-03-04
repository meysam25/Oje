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
    [AreaConfig(ModualTitle = " پایه استعلام ثالث خودرو", Icon = "fa-car-side", Title = "سابقه خسارت راننده")]
    [CustomeAuthorizeFilter]
    public class ThirdPartyDriverHistoryDamagePenaltyController: Controller
    {
        readonly IThirdPartyDriverHistoryDamagePenaltyService ThirdPartyDriverHistoryDamagePenaltyService = null;
        public ThirdPartyDriverHistoryDamagePenaltyController(
                IThirdPartyDriverHistoryDamagePenaltyService ThirdPartyDriverHistoryDamagePenaltyService
            )
        {
            this.ThirdPartyDriverHistoryDamagePenaltyService = ThirdPartyDriverHistoryDamagePenaltyService;
        }

        [AreaConfig(Title = "سابقه خسارت راننده", Icon = "fa-history", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "سابقه خسارت راننده";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ThirdPartyDriverHistoryDamagePenalty", new { area = "CarThirdBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست سابقه خسارت راننده", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("CarThirdBaseData", "ThirdPartyDriverHistoryDamagePenalty")));
        }

        [AreaConfig(Title = "افزودن سابقه خسارت راننده جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateThirdPartyDriverHistoryDamagePenaltyVM input)
        {
            return Json(ThirdPartyDriverHistoryDamagePenaltyService.Create(input));
        }

        [AreaConfig(Title = "حذف سابقه خسارت راننده", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(ThirdPartyDriverHistoryDamagePenaltyService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک سابقه خسارت راننده", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(ThirdPartyDriverHistoryDamagePenaltyService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  سابقه خسارت راننده", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateThirdPartyDriverHistoryDamagePenaltyVM input)
        {
            return Json(ThirdPartyDriverHistoryDamagePenaltyService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست سابقه خسارت راننده", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] ThirdPartyDriverHistoryDamagePenaltyMainGrid searchInput)
        {
            return Json(ThirdPartyDriverHistoryDamagePenaltyService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ThirdPartyDriverHistoryDamagePenaltyMainGrid searchInput)
        {
            var result = ThirdPartyDriverHistoryDamagePenaltyService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
