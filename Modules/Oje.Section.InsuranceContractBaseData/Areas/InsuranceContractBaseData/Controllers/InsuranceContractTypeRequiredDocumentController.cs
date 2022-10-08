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
    [AreaConfig(ModualTitle = "مدیریت قرارداد ها و مجوز ها", Icon = "fa-file-invoice", Title = "مدارک مورد نیاز فرم پیشنهاد")]
    [CustomeAuthorizeFilter]
    public class InsuranceContractTypeRequiredDocumentController : Controller
    {
        readonly IInsuranceContractTypeRequiredDocumentService InsuranceContractTypeRequiredDocumentService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IInsuranceContractService InsuranceContractService = null;
        readonly IInsuranceContractTypeService InsuranceContractTypeService = null;

        public InsuranceContractTypeRequiredDocumentController(
                IInsuranceContractTypeRequiredDocumentService InsuranceContractTypeRequiredDocumentService,
                ISiteSettingService SiteSettingService,
                IInsuranceContractService InsuranceContractService,
                IInsuranceContractTypeService InsuranceContractTypeService
            )
        {
            this.InsuranceContractTypeRequiredDocumentService = InsuranceContractTypeRequiredDocumentService;
            this.SiteSettingService = SiteSettingService;
            this.InsuranceContractService = InsuranceContractService;
            this.InsuranceContractTypeService = InsuranceContractTypeService;
        }

        [AreaConfig(Title = "مدارک مورد نیاز فرم پیشنهاد", Icon = "fa-file-image", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "مدارک مورد نیاز فرم پیشنهاد";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "InsuranceContractTypeRequiredDocument", new { area = "InsuranceContractBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه مدارک مورد نیاز فرم پیشنهاد", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("InsuranceContractBaseData", "InsuranceContractTypeRequiredDocument")));
        }

        [AreaConfig(Title = "افزودن مدارک مورد نیاز فرم پیشنهاد جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] InsuranceContractTypeRequiredDocumentCreateUpdateVM input)
        {
            return Json(InsuranceContractTypeRequiredDocumentService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف مدارک مورد نیاز فرم پیشنهاد", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(InsuranceContractTypeRequiredDocumentService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک مدارک مورد نیاز فرم پیشنهاد", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(InsuranceContractTypeRequiredDocumentService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  مدارک مورد نیاز فرم پیشنهاد", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] InsuranceContractTypeRequiredDocumentCreateUpdateVM input)
        {
            return Json(InsuranceContractTypeRequiredDocumentService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست مدارک مورد نیاز فرم پیشنهاد", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] InsuranceContractTypeRequiredDocumentMainGrid searchInput)
        {
            return Json(InsuranceContractTypeRequiredDocumentService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] InsuranceContractTypeRequiredDocumentMainGrid searchInput)
        {
            var result = InsuranceContractTypeRequiredDocumentService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "لیست نوع قرارداد", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetContractTypeList([FromQuery]int? id, [FromQuery] int? cSOWSiteSettingId)
        {
            return Json(InsuranceContractTypeService.GetLightList(id, HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "لیست قرارداد", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetContractList([FromQuery] int? cSOWSiteSettingId)
        {
            return Json(InsuranceContractService.GetLightList(HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
