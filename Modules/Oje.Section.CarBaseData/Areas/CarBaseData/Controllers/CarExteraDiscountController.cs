﻿using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.CarBaseData.Interfaces;
using Oje.Section.CarBaseData.Models.View;
using Microsoft.AspNetCore.Mvc;
using System;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.CarBaseData.Areas.CarBaseData.Controllers
{
    [Area("CarBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "پایه خودرو", Icon = "fa-car", Title = "تخفیف اضافه")]
    [CustomeAuthorizeFilter]
    public class CarExteraDiscountController : Controller
    {
        readonly ICarExteraDiscountService CarExteraDiscountService = null;
        readonly IProposalFormService ProposalFormService = null;
        readonly ICarExteraDiscountCategoryService CarExteraDiscountCategoryService = null;
        readonly IVehicleTypeService VehicleTypeService = null;
        public CarExteraDiscountController(
            ICarExteraDiscountService CarExteraDiscountService,
            IProposalFormService ProposalFormService,
            ICarExteraDiscountCategoryService CarExteraDiscountCategoryService,
            IVehicleTypeService VehicleTypeService
            )
        {
            this.CarExteraDiscountService = CarExteraDiscountService;
            this.ProposalFormService = ProposalFormService;
            this.CarExteraDiscountCategoryService = CarExteraDiscountCategoryService;
            this.VehicleTypeService = VehicleTypeService;
        }

        [AreaConfig(Title = "تخفیف اضافه", Icon = "fa-tag", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "تخفیف اضافه";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "CarExteraDiscount", new { area = "CarBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست تخفیف اضافه", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("CarBaseData", "CarExteraDiscount")));
        }

        [AreaConfig(Title = "افزودن تخفیف اضافه جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateCarExteraDiscountVM input)
        {
            return Json(CarExteraDiscountService.Create(input));
        }

        [AreaConfig(Title = "حذف تخفیف اضافه", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(CarExteraDiscountService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک تخفیف اضافه", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(CarExteraDiscountService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  تخفیف اضافه", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateCarExteraDiscountVM input)
        {
            return Json(CarExteraDiscountService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست تخفیف اضافه", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] CarExteraDiscountMainGrid searchInput)
        {
            return Json(CarExteraDiscountService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] CarExteraDiscountMainGrid searchInput)
        {
            var result = CarExteraDiscountService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست نوع خودرو", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCarTypeList()
        {
            return Json(VehicleTypeService.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست گروه بندی تخفیف", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetDiscountCatList()
        {
            return Json(CarExteraDiscountCategoryService.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست فرم های پیشنهاد", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetCProposalFormList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(ProposalFormService.GetSelect2List(searchInput));
        }
    }
}
