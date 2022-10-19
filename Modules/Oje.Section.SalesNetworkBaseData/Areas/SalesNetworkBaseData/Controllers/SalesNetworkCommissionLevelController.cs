using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.SalesNetworkBaseData.Models.View;
using Oje.Section.SalesNetworkBaseData.Services;
using System;
using Oje.Section.SalesNetworkBaseData.Interfaces;
using Oje.AccountService.Interfaces;

namespace Oje.Section.SalesNetworkBaseData.Areas.SalesNetworkBaseData.Controllers
{
    [Area("SalesNetworkBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات نرخ شبکه فروش چند سطحی/بازاریاب", Icon = "fa-network-wired", Title = "نرخ شبکه فروش چند سطحی")]
    [CustomeAuthorizeFilter]
    public class SalesNetworkCommissionLevelController: Controller
    {
        readonly ISalesNetworkCommissionLevelService SalesNetworkCommissionLevelService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly ISalesNetworkService SalesNetworkService = null;

        public SalesNetworkCommissionLevelController
            (
                ISalesNetworkCommissionLevelService SalesNetworkCommissionLevelService,
                ISiteSettingService SiteSettingService,
                ISalesNetworkService SalesNetworkService
            )
        {
            this.SalesNetworkCommissionLevelService = SalesNetworkCommissionLevelService;
            this.SiteSettingService = SiteSettingService;
            this.SalesNetworkService = SalesNetworkService;
        }

        [AreaConfig(Title = "نرخ شبکه فروش چند سطحی", Icon = "fa-percentage", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "نرخ شبکه فروش چند سطحی";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "SalesNetworkCommissionLevel", new { area = "SalesNetworkBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست نرخ شبکه فروش چند سطحی", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("SalesNetworkBaseData", "SalesNetworkCommissionLevel")));
        }

        [AreaConfig(Title = "افزودن نرخ شبکه فروش چند سطحی جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] SalesNetworkCommissionLevelCreateUpdateVM input)
        {
            return Json(SalesNetworkCommissionLevelService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف نرخ شبکه فروش چند سطحی", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(SalesNetworkCommissionLevelService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک نرخ شبکه فروش چند سطحی", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(SalesNetworkCommissionLevelService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  نرخ شبکه فروش چند سطحی", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] SalesNetworkCommissionLevelCreateUpdateVM input)
        {
            return Json(SalesNetworkCommissionLevelService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست نرخ شبکه فروش چند سطحی", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] SalesNetworkCommissionLevelMainGrid searchInput)
        {
            return Json(SalesNetworkCommissionLevelService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] SalesNetworkCommissionLevelMainGrid searchInput)
        {
            var result = SalesNetworkCommissionLevelService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

       
        [AreaConfig(Title = "مشاهده لیست شبکه های فروش", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetSalesNetworkList([FromQuery] int? cSOWSiteSettingId)
        {
            return Json(SalesNetworkService.GetLightListMultiLevel(HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
