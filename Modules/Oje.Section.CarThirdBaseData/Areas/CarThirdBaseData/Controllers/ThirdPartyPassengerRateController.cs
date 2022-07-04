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
    [AreaConfig(ModualTitle = " پایه استعلام ثالث خودرو (ادمین)", Icon = "fa-car-side", Title = "نرخ سرنشین ثالث")]
    [CustomeAuthorizeFilter]
    public class ThirdPartyPassengerRateController : Controller
    {
        readonly IThirdPartyPassengerRateService ThirdPartyPassengerRateService = null;
        readonly ICompanyService CompanyService = null;
        readonly ICarSpecificationService CarSpecificationService = null;
        public ThirdPartyPassengerRateController(
            IThirdPartyPassengerRateService ThirdPartyPassengerRateService,
            ICompanyService CompanyService,
            ICarSpecificationService CarSpecificationService
            )
        {
            this.ThirdPartyPassengerRateService = ThirdPartyPassengerRateService;
            this.CompanyService = CompanyService;
            this.CarSpecificationService = CarSpecificationService;
        }

        [AreaConfig(Title = "نرخ سرنشین ثالث", Icon = "fa-comments-dollar", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "نرخ سرنشین ثالث";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ThirdPartyPassengerRate", new { area = "CarThirdBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست نرخ سرنشین ثالث", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("CarThirdBaseData", "ThirdPartyPassengerRate")));
        }

        [AreaConfig(Title = "افزودن نرخ سرنشین ثالث جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateThirdPartyPassengerRateVM input)
        {
            return Json(ThirdPartyPassengerRateService.Create(input));
        }

        [AreaConfig(Title = "حذف نرخ سرنشین ثالث", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(ThirdPartyPassengerRateService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک نرخ سرنشین ثالث", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(ThirdPartyPassengerRateService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  نرخ سرنشین ثالث", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateThirdPartyPassengerRateVM input)
        {
            return Json(ThirdPartyPassengerRateService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست نرخ سرنشین ثالث", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] ThirdPartyPassengerRateMainGrid searchInput)
        {
            return Json(ThirdPartyPassengerRateService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ThirdPartyPassengerRateMainGrid searchInput)
        {
            var result = ThirdPartyPassengerRateService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست شرکت", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCompanyList()
        {
            return Json(CompanyService.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست خصوصیات خودرو", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetCarSepecificationList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(CarSpecificationService.GetSelect2List(searchInput));
        }
    }
}
