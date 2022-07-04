using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.InsuranceContractBaseData.Interfaces;
using Oje.Section.InsuranceContractBaseData.Models.View;
using System;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.InsuranceContractBaseData.Areas.InsuranceContractBaseData.Controllers
{
    [Area("InsuranceContractBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "مدیریت قرارداد ها و مجوز ها", Icon = "fa-file-invoice", Title = "لیست زیرگوره بیمه شدگان")]
    [CustomeAuthorizeFilter]
    public class InsuranceContractUserSubCategoryController : Controller
    {
        readonly IInsuranceContractUserSubCategoryService InsuranceContractUserSubCategoryService = null;
        readonly ISiteSettingService SiteSettingService = null;

        public InsuranceContractUserSubCategoryController
            (
                IInsuranceContractUserSubCategoryService InsuranceContractUserSubCategoryService,
                ISiteSettingService SiteSettingService
            )
        {
            this.InsuranceContractUserSubCategoryService = InsuranceContractUserSubCategoryService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "لیست زیرگوره بیمه شدگان", Icon = "fa-object-group", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "لیست زیرگوره بیمه شدگان";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "InsuranceContractUserSubCategory", new { area = "InsuranceContractBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست زیرگوره بیمه شدگان", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("InsuranceContractBaseData", "InsuranceContractUserSubCategory")));
        }

        [AreaConfig(Title = "افزودن لیست زیرگوره بیمه شدگان جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] InsuranceContractUserSubCategoryCreateUpdateVM input)
        {
            return Json(InsuranceContractUserSubCategoryService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف زیرگوره بیمه شدگان", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(InsuranceContractUserSubCategoryService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده یک  زیرگوره بیمه شدگان", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(InsuranceContractUserSubCategoryService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی زیرگوره بیمه شدگان", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] InsuranceContractUserSubCategoryCreateUpdateVM input)
        {
            return Json(InsuranceContractUserSubCategoryService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست زیرگوره بیمه شدگان", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] InsuranceContractUserSubCategoryMainGrid searchInput)
        {
            return Json(InsuranceContractUserSubCategoryService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] InsuranceContractUserSubCategoryMainGrid searchInput)
        {
            var result = InsuranceContractUserSubCategoryService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
