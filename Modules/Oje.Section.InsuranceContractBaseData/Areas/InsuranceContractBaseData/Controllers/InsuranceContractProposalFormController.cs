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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Areas.InsuranceContractBaseData.Controllers
{
    [Area("InsuranceContractBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "مدیریت قرارداد ها و مجوز ها", Icon = "fa-file-powerpoint", Title = "فرم پیشنهاد تفاهم نامه")]
    [CustomeAuthorizeFilter]
    public class InsuranceContractProposalFormController : Controller
    {
        readonly IInsuranceContractProposalFormService InsuranceContractProposalFormService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public InsuranceContractProposalFormController(IInsuranceContractProposalFormService InsuranceContractProposalFormService, ISiteSettingService SiteSettingService)
        {
            this.InsuranceContractProposalFormService = InsuranceContractProposalFormService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "فرم پیشنهاد تفاهم نامه", Icon = "fa-file-alt", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "فرم پیشنهاد تفاهم نامه";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "InsuranceContractProposalForm", new { area = "InsuranceContractBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست فرم های پیشنهاد تفاهم نامه", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("InsuranceContractBaseData", "InsuranceContractProposalForm")));
        }

        [AreaConfig(Title = "افزودن فرم های پیشنهاد تفاهم نامه جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] InsuranceContractProposalFormCreateUpdateVM input)
        {
            return Json(InsuranceContractProposalFormService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف فرم های پیشنهاد تفاهم نامه", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(InsuranceContractProposalFormService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک فرم های پیشنهاد تفاهم نامه", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(InsuranceContractProposalFormService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  فرم های پیشنهاد تفاهم نامه", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] InsuranceContractProposalFormCreateUpdateVM input)
        {
            return Json(InsuranceContractProposalFormService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست فرم های پیشنهاد تفاهم نامه", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] InsuranceContractProposalFormMainGrid searchInput)
        {
            return Json(InsuranceContractProposalFormService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] InsuranceContractProposalFormMainGrid searchInput)
        {
            var result = InsuranceContractProposalFormService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
