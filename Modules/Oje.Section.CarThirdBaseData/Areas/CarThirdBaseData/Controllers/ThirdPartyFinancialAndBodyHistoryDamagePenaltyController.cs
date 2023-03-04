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
    [AreaConfig(ModualTitle = " پایه استعلام ثالث خودرو", Icon = "fa-car-side", Title = "سابقه خسارت جانی و مالی")]
    [CustomeAuthorizeFilter]
    public class ThirdPartyFinancialAndBodyHistoryDamagePenaltyController: Controller
    {
        readonly IThirdPartyFinancialAndBodyHistoryDamagePenaltyService ThirdPartyFinancialAndBodyHistoryDamagePenaltyService = null;
        public ThirdPartyFinancialAndBodyHistoryDamagePenaltyController(
                IThirdPartyFinancialAndBodyHistoryDamagePenaltyService ThirdPartyFinancialAndBodyHistoryDamagePenaltyService
            )
        {
            this.ThirdPartyFinancialAndBodyHistoryDamagePenaltyService = ThirdPartyFinancialAndBodyHistoryDamagePenaltyService;
        }

        [AreaConfig(Title = "سابقه خسارت جانی و مالی", Icon = "fa-history", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "سابقه خسارت جانی و مالی";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ThirdPartyFinancialAndBodyHistoryDamagePenalty", new { area = "CarThirdBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست سابقه خسارت جانی و مالی", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("CarThirdBaseData", "ThirdPartyFinancialAndBodyHistoryDamagePenalty")));
        }

        [AreaConfig(Title = "افزودن سابقه خسارت جانی و مالی جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateThirdPartyFinancialAndBodyHistoryDamagePenaltyVM input)
        {
            return Json(ThirdPartyFinancialAndBodyHistoryDamagePenaltyService.Create(input));
        }

        [AreaConfig(Title = "حذف سابقه خسارت جانی و مالی", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(ThirdPartyFinancialAndBodyHistoryDamagePenaltyService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک سابقه خسارت جانی و مالی", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(ThirdPartyFinancialAndBodyHistoryDamagePenaltyService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  سابقه خسارت جانی و مالی", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateThirdPartyFinancialAndBodyHistoryDamagePenaltyVM input)
        {
            return Json(ThirdPartyFinancialAndBodyHistoryDamagePenaltyService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست سابقه خسارت جانی و مالی", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] ThirdPartyFinancialAndBodyHistoryDamagePenaltyMainGrid searchInput)
        {
            return Json(ThirdPartyFinancialAndBodyHistoryDamagePenaltyService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ThirdPartyFinancialAndBodyHistoryDamagePenaltyMainGrid searchInput)
        {
            var result = ThirdPartyFinancialAndBodyHistoryDamagePenaltyService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
