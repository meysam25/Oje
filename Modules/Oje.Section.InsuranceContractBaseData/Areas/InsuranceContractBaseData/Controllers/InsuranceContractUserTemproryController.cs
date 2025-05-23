﻿using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.InsuranceContractBaseData.Interfaces;
using Oje.Section.InsuranceContractBaseData.Models.View;
using Microsoft.AspNetCore.Mvc;
using System;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Exceptions;
using Oje.PaymentService.Interfaces;

namespace Oje.Section.InsuranceContractBaseData.Areas.InsuranceContractBaseData.Controllers
{
    [Area("InsuranceContractBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "سامانه درمان وب", Order = 5, Icon = "fa-file-invoice", Title = "لیست بیمه شدگان موقت")]
    [CustomeAuthorizeFilter]
    public class InsuranceContractUserTemproryController : Controller
    {
        readonly IInsuranceContractUserService InsuranceContractUserService = null;
        readonly IInsuranceContractService InsuranceContractService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IInsuranceContractUserBaseInsuranceService InsuranceContractUserBaseInsuranceService = null;
        readonly IInsuranceContractUserSubCategoryService InsuranceContractUserSubCategoryService = null;
        readonly IBankService BankService = null;

        public InsuranceContractUserTemproryController(
                IInsuranceContractUserService InsuranceContractUserService,
                IInsuranceContractService InsuranceContractService,
                ISiteSettingService SiteSettingService,
                IInsuranceContractUserBaseInsuranceService InsuranceContractUserBaseInsuranceService,
                IInsuranceContractUserSubCategoryService InsuranceContractUserSubCategoryService,
                IBankService BankService
            )
        {
            this.InsuranceContractUserService = InsuranceContractUserService;
            this.InsuranceContractService = InsuranceContractService;
            this.SiteSettingService = SiteSettingService;
            this.InsuranceContractUserBaseInsuranceService = InsuranceContractUserBaseInsuranceService;
            this.InsuranceContractUserSubCategoryService = InsuranceContractUserSubCategoryService;
            this.BankService = BankService;
        }

        [AreaConfig(Title = "لیست بیمه شدگان موقت", Icon = "fa-user-alt", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "لیست بیمه شدگان موقت";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "InsuranceContractUserTemprory", new { area = "InsuranceContractBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست بیمه شدگان موقت", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("InsuranceContractBaseData", "InsuranceContractUserTemprory")));
        }

        [AreaConfig(Title = "تایید بیمه شدگان موقت", Icon = "fa-check")]
        [HttpPost]
        public IActionResult Confirm([FromForm] GlobalLongId input)
        {
            return Json(InsuranceContractUserService.ChangeStatus(input?.id, InsuranceContractUserStatus.Temprory, InsuranceContractUserStatus.Premanent));
        }

        [AreaConfig(Title = "افزودن لیست بیمه شدگان موقت جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateInsuranceContractUserVM input)
        {
            return Json(InsuranceContractUserService.Create(input, InsuranceContractUserStatus.Temprory));
        }

        [AreaConfig(Title = "افزودن بیمه شدگان موقت از روی فایل اکسل", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult CreateFromXcel([FromForm] GlobalExcelFile input)
        {
            return Json(InsuranceContractUserService.CreateFromExcel(input, InsuranceContractUserStatus.Temprory, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "افزودن بیمه شدگان موقت زیرمجموعه از روی فایل اکسل", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult CreateFromXcelChilds([FromForm] GlobalExcelFile input)
        {
            return Json(InsuranceContractUserService.CreateFromExcelChild(input, InsuranceContractUserStatus.Temprory, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف بیمه شدگان موقت", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(InsuranceContractUserService.Delete(input?.id, InsuranceContractUserStatus.Temprory));
        }

        [AreaConfig(Title = "مشاهده یک  بیمه شدگان موقت", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalLongId input)
        {
            return Json(InsuranceContractUserService.GetById(input?.id, InsuranceContractUserStatus.Temprory));
        }

        [AreaConfig(Title = "به روز رسانی بیمه شدگان موقت", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateInsuranceContractUserVM input)
        {
            return Json(InsuranceContractUserService.Update(input, InsuranceContractUserStatus.Temprory));
        }

        [AreaConfig(Title = "مشاهده لیست بیمه شدگان موقت", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] InsuranceContractUserMainGrid searchInput)
        {
            return Json(InsuranceContractUserService.GetList(searchInput, InsuranceContractUserStatus.Temprory));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] InsuranceContractUserMainGrid searchInput)
        {
            var result = InsuranceContractUserService.GetList(searchInput, InsuranceContractUserStatus.Temprory);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست قرارداد", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetContractList([FromQuery] int? cSOWSiteSettingId)
        {
            return Json(InsuranceContractService.GetLightList(HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست بیمه پایه", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetBaseInsuranceList([FromQuery] int? cSOWSiteSettingId)
        {
            return Json(InsuranceContractUserBaseInsuranceService.GetLightList(HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست زیرگروه", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetSubCatList([FromQuery] int? cSOWSiteSettingId)
        {
            return Json(InsuranceContractUserSubCategoryService.GetLightList(HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست بانک", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetBankList()
        {
            return Json(BankService.GetLightList());
        }
    }
}
