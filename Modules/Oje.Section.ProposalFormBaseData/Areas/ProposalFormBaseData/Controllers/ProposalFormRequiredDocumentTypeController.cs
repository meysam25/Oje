using Oje.AccountManager.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.ProposalFormBaseData.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using Oje.Section.ProposalFormBaseData.Models.View;

namespace Oje.Section.ProposalFormBaseData.Areas.ProposalFormBaseData.Controllers
{
    [Area("ProposalFormBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات فرم پیشنهاد", Icon = "fa-file-powerpoint", Title = "نوع مدارک مورد نیاز")]
    [CustomeAuthorizeFilter]
    public class ProposalFormRequiredDocumentTypeController: Controller
    {
        readonly IProposalFormRequiredDocumentTypeManager ProposalFormRequiredDocumentTypeManager = null;
        readonly IProposalFormManager ProposalFormManager = null;
        readonly ISiteSettingManager SiteSettingManager = null;
        public ProposalFormRequiredDocumentTypeController(
            IProposalFormRequiredDocumentTypeManager ProposalFormRequiredDocumentTypeManager, 
            IProposalFormManager ProposalFormManager, 
            ISiteSettingManager SiteSettingManager
            )
        {
            this.ProposalFormRequiredDocumentTypeManager = ProposalFormRequiredDocumentTypeManager;
            this.ProposalFormManager = ProposalFormManager;
            this.SiteSettingManager = SiteSettingManager;
        }

        [AreaConfig(Title = "نوع مدارک مورد نیاز", Icon = "fa-object-group", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "نوع مدارک مورد نیاز";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ProposalFormRequiredDocumentType", new { area = "ProposalFormBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست نوع مدارک مورد نیاز", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("ProposalFormBaseData", "ProposalFormRequiredDocumentType")));
        }

        [AreaConfig(Title = "افزودن نوع مدارک مورد نیاز جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateProposalFormRequiredDocumentTypeVM input)
        {
            return Json(ProposalFormRequiredDocumentTypeManager.Create(input));
        }

        [AreaConfig(Title = "حذف نوع مدارک مورد نیاز", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(ProposalFormRequiredDocumentTypeManager.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک نوع مدارک مورد نیاز", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(ProposalFormRequiredDocumentTypeManager.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  نوع مدارک مورد نیاز", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateProposalFormRequiredDocumentTypeVM input)
        {
            return Json(ProposalFormRequiredDocumentTypeManager.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست نوع مدارک مورد نیاز", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] ProposalFormRequiredDocumentTypeMainGrid searchInput)
        {
            return Json(ProposalFormRequiredDocumentTypeManager.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ProposalFormRequiredDocumentTypeMainGrid searchInput)
        {
            var result = ProposalFormRequiredDocumentTypeManager.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست وب سایت ها", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetSiteList()
        {
            return Json(SiteSettingManager.GetLightList());
        }

        [AreaConfig(Title = "مشاهده لیست فرم های پیشنهاد", Icon = "fa-list-alt ")]
        [HttpGet]
        public ActionResult GetProposalFormList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(ProposalFormManager.GetSelect2List(searchInput));
        }
    }
}
