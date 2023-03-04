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
    [AreaConfig(ModualTitle = " پایه استعلام ثالث خودرو", Icon = "fa-car-side", Title = "سرمایه بیمه ثالث")]
    [CustomeAuthorizeFilter]
    public class ThirdPartyRateController: Controller
    {
        readonly IThirdPartyRateService ThirdPartyRateService = null;
        readonly ICompanyService CompanyService = null;
        readonly ICarSpecificationService CarSpecificationService = null;
        public ThirdPartyRateController(
            IThirdPartyRateService ThirdPartyRateService,
            ICompanyService CompanyService,
            ICarSpecificationService CarSpecificationService
            )
        {
            this.ThirdPartyRateService = ThirdPartyRateService;
            this.CompanyService = CompanyService;
            this.CarSpecificationService = CarSpecificationService;
        }

        [AreaConfig(Title = "سرمایه بیمه ثالث", Icon = "fa-dollar-sign", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "سرمایه بیمه ثالث";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ThirdPartyRate", new { area = "CarThirdBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست سرمایه بیمه ثالث", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("CarThirdBaseData", "ThirdPartyRate")));
        }

        [AreaConfig(Title = "افزودن سرمایه بیمه ثالث جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateThirdPartyRateVM input)
        {
            return Json(ThirdPartyRateService.Create(input));
        }

        [AreaConfig(Title = "حذف سرمایه بیمه ثالث", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(ThirdPartyRateService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک سرمایه بیمه ثالث", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(ThirdPartyRateService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  سرمایه بیمه ثالث", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateThirdPartyRateVM input)
        {
            return Json(ThirdPartyRateService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست سرمایه بیمه ثالث", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] ThirdPartyRateMainGrid searchInput)
        {
            return Json(ThirdPartyRateService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ThirdPartyRateMainGrid searchInput)
        {
            var result = ThirdPartyRateService.GetList(searchInput);
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
