using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.CarThirdBaseData.Interfaces;
using Oje.Section.CarThirdBaseData.Models.View;
using System;
using Oje.AccountService.Interfaces;
using ICompanyService = Oje.Section.CarThirdBaseData.Interfaces.ICompanyService;

namespace Oje.Section.CarThirdBaseData.Areas.CarThirdBaseData.Controllers
{

    [Area("CarThirdBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = " پایه استعلام ثالث خودرو", Icon = "fa-car-side", Title = "درصد تخفیف تعهدات مالی بر اساس نوع خودرو")]
    [CustomeAuthorizeFilter]
    public class ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountController: Controller
    {
        readonly IThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountService ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IVehicleTypeService VehicleTypeService = null;
        readonly ICompanyService CompanyService = null;

        public ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountController
            (
                IThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountService ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountService,
                ISiteSettingService SiteSettingService,
                IVehicleTypeService VehicleTypeService,
                ICompanyService CompanyService
            )
        {
            this.ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountService = ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountService;
            this.SiteSettingService = SiteSettingService;
            this.VehicleTypeService = VehicleTypeService;
            this.CompanyService = CompanyService;
        }

        [AreaConfig(Title = "درصد تخفیف تعهدات مالی بر اساس نوع خودرو", Icon = "fa-percent", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "درصد تخفیف تعهدات مالی بر اساس نوع خودرو";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscount", new { area = "CarThirdBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست درصد تخفیف تعهدات مالی بر اساس نوع خودرو", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("CarThirdBaseData", "ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscount")));
        }

        [AreaConfig(Title = "افزودن درصد تخفیف تعهدات مالی بر اساس نوع خودرو جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCreateUpdateVM input)
        {
            return Json(ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف درصد تخفیف تعهدات مالی بر اساس نوع خودرو", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک درصد تخفیف تعهدات مالی بر اساس نوع خودرو", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  درصد تخفیف تعهدات مالی بر اساس نوع خودرو", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCreateUpdateVM input)
        {
            return Json(ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست درصد تخفیف تعهدات مالی بر اساس نوع خودرو", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountMainGrid searchInput)
        {
            return Json(ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountMainGrid searchInput)
        {
            var result = ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "لیست نوع خودرو", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetVicleList()
        {
            return Json(VehicleTypeService.GetLightList());
        }

        [AreaConfig(Title = "لیست شرکت", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCompanyList()
        {
            return Json(CompanyService.GetLightList());
        }
    }
}
