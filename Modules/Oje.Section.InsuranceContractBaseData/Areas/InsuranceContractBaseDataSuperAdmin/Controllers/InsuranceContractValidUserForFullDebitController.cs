﻿using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.InsuranceContractBaseData.Interfaces;
using Oje.Section.InsuranceContractBaseData.Models.View;
using Microsoft.AspNetCore.Mvc;
using System;
using Oje.Infrastructure.Exceptions;
using Oje.AccountService.Interfaces;

namespace Oje.Section.InsuranceContractBaseData.Areas.InsuranceContractBaseDataSuperAdmin.Controllers
{
    [Area("InsuranceContractBaseDataSuperAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "مدیریت قرارداد ها و مجوز ها", Icon = "fa-file-invoice", Title = "فروش از دم قسط")]
    [CustomeAuthorizeFilter]
    public class InsuranceContractValidUserForFullDebitController : Controller
    {
        readonly IInsuranceContractValidUserForFullDebitService InsuranceContractValidUserForFullDebitService = null;
        readonly IInsuranceContractService InsuranceContractService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public InsuranceContractValidUserForFullDebitController
            (
                IInsuranceContractValidUserForFullDebitService InsuranceContractValidUserForFullDebitService,
                IInsuranceContractService InsuranceContractService,
                ISiteSettingService SiteSettingService
            )
        {
            this.InsuranceContractValidUserForFullDebitService = InsuranceContractValidUserForFullDebitService;
            this.InsuranceContractService = InsuranceContractService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "فروش از دم قسط", Icon = "fa-credit-card", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "فروش از دم قسط";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "InsuranceContractValidUserForFullDebit", new { area = "InsuranceContractBaseDataSuperAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست فروش از دم قسط", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("InsuranceContractBaseDataSuperAdmin", "InsuranceContractValidUserForFullDebit")));
        }

        [AreaConfig(Title = "افزودن فروش از دم قسط جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateInsuranceContractValidUserForFullDebitVM input)
        {
            return Json(InsuranceContractValidUserForFullDebitService.Create(input));
        }

        [AreaConfig(Title = "افزودن فروش از دم قسط از روی فایل اکسل", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult CreateFromXcel([FromForm] GlobalExcelFile input)
        {
            return Json(InsuranceContractValidUserForFullDebitService.CreateFromExcel(input));
        }

        [AreaConfig(Title = "حذف فروش از دم قسط", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(InsuranceContractValidUserForFullDebitService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک فروش از دم قسط", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalLongId input)
        {
            return Json(InsuranceContractValidUserForFullDebitService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  فروش از دم قسط", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateInsuranceContractValidUserForFullDebitVM input)
        {
            return Json(InsuranceContractValidUserForFullDebitService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست فروش از دم قسط", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] InsuranceContractValidUserForFullDebitMainGrid searchInput)
        {
            return Json(InsuranceContractValidUserForFullDebitService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] InsuranceContractValidUserForFullDebitMainGrid searchInput)
        {
            var result = InsuranceContractValidUserForFullDebitService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);


            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده قرارداد", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetContractList([FromQuery] int? cSOWSiteSettingId)
        {
            return Json(InsuranceContractService.GetLightList(HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
