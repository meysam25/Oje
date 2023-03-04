using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Services;
using Oje.Section.InsuranceContractBaseData.Interfaces;
using Oje.Section.InsuranceContractBaseData.Models.View;
using System;

namespace Oje.Section.InsuranceContractBaseData.Areas.InsuranceContractBaseDataSuperAdmin.Controllers
{
    [Area("InsuranceContractBaseDataSuperAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "مدیریت قرارداد ها و مجوز ها", Icon = "fa-file-invoice", Title = "حداکثر غرامت")]
    [CustomeAuthorizeFilter]
    public class InsuranceContractInsuranceContractTypeMaxPriceController : Controller
    {
        readonly IInsuranceContractInsuranceContractTypeMaxPriceService InsuranceContractInsuranceContractTypeMaxPriceService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IInsuranceContractTypeService InsuranceContractTypeService = null;
        readonly IInsuranceContractService InsuranceContractService = null;

        public InsuranceContractInsuranceContractTypeMaxPriceController
            (
                IInsuranceContractInsuranceContractTypeMaxPriceService InsuranceContractInsuranceContractTypeMaxPriceService,
                ISiteSettingService SiteSettingService,
                IInsuranceContractTypeService InsuranceContractTypeService,
                IInsuranceContractService InsuranceContractService
            )
        {
            this.InsuranceContractInsuranceContractTypeMaxPriceService = InsuranceContractInsuranceContractTypeMaxPriceService;
            this.SiteSettingService = SiteSettingService;
            this.InsuranceContractTypeService = InsuranceContractTypeService;
            this.InsuranceContractService = InsuranceContractService;
        }

        [AreaConfig(Title = "لیست حداکثر غرامت", Icon = "fa-money-check-edit-alt", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "لیست حداکثر غرامت";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "InsuranceContractInsuranceContractTypeMaxPrice", new { area = "InsuranceContractBaseDataSuperAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست حداکثر غرامت", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("InsuranceContractBaseDataSuperAdmin", "InsuranceContractInsuranceContractTypeMaxPrice")));
        }

        [AreaConfig(Title = "افزودن لیست حداکثر غرامت جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] InsuranceContractInsuranceContractTypeMaxPriceCreateUpdateVM input)
        {
            return Json(InsuranceContractInsuranceContractTypeMaxPriceService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف حداکثر غرامت", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] string id)
        {
            return Json(InsuranceContractInsuranceContractTypeMaxPriceService.Delete(id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده یک  حداکثر غرامت", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] string id)
        {
            return Json(InsuranceContractInsuranceContractTypeMaxPriceService.GetById(id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی حداکثر غرامت", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] InsuranceContractInsuranceContractTypeMaxPriceCreateUpdateVM input)
        {
            return Json(InsuranceContractInsuranceContractTypeMaxPriceService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست حداکثر غرامت", Icon = "fa-list-alt")]
        [HttpPost]
        public IActionResult GetList([FromForm] InsuranceContractInsuranceContractTypeMaxPriceMainGrid searchInput)
        {
            return Json(InsuranceContractInsuranceContractTypeMaxPriceService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public IActionResult Export([FromForm] InsuranceContractInsuranceContractTypeMaxPriceMainGrid searchInput)
        {
            var result = InsuranceContractInsuranceContractTypeMaxPriceService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست نوع", Icon = "fa-list-alt")]
        [HttpPost]
        public IActionResult GetTypeList([FromQuery] int? cSOWSiteSettingId)
        {
            return Json(InsuranceContractTypeService.GetLightListBySiteSettingId(HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست قرارداد", Icon = "fa-list-alt")]
        [HttpPost]
        public IActionResult GetContractList([FromQuery] int? cSOWSiteSettingId)
        {
            return Json(InsuranceContractService.GetLightList(HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));

        }
    }
}
