using Oje.AccountService.Filters;
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
    [AreaConfig(ModualTitle = "تنظیمات فرم پیشنهاد", Icon = "fa-file-powerpoint", Title = "مدارک مورد نیاز فرم پیشنهاد")]
    [CustomeAuthorizeFilter]
    public class ProposalFormRequiredDocumentController: Controller
    {
        readonly IProposalFormRequiredDocumentService ProposalFormRequiredDocumentService = null;
        readonly IProposalFormRequiredDocumentTypeService ProposalFormRequiredDocumentTypeService = null;
        public ProposalFormRequiredDocumentController(
                IProposalFormRequiredDocumentService ProposalFormRequiredDocumentService,
                IProposalFormRequiredDocumentTypeService ProposalFormRequiredDocumentTypeService
            )
        {
            this.ProposalFormRequiredDocumentService = ProposalFormRequiredDocumentService;
            this.ProposalFormRequiredDocumentTypeService = ProposalFormRequiredDocumentTypeService;
        }

        [AreaConfig(Title = "مدارک مورد نیاز فرم پیشنهاد", Icon = "fa-file-image", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "مدارک مورد نیاز فرم پیشنهاد";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ProposalFormRequiredDocument", new { area = "ProposalFormBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه مدارک مورد نیاز فرم پیشنهاد", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("ProposalFormBaseData", "ProposalFormRequiredDocument")));
        }

        [AreaConfig(Title = "افزودن مدارک مورد نیاز فرم پیشنهاد جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateProposalFormRequiredDocumentVM input)
        {
            return Json(ProposalFormRequiredDocumentService.Create(input, HttpContext.GetLoginUserId()?.UserId));
        }

        [AreaConfig(Title = "حذف مدارک مورد نیاز فرم پیشنهاد", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(ProposalFormRequiredDocumentService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک مدارک مورد نیاز فرم پیشنهاد", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(ProposalFormRequiredDocumentService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  مدارک مورد نیاز فرم پیشنهاد", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateProposalFormRequiredDocumentVM input)
        {
            return Json(ProposalFormRequiredDocumentService.Update(input, HttpContext.GetLoginUserId()?.UserId));
        }

        [AreaConfig(Title = "مشاهده لیست مدارک مورد نیاز فرم پیشنهاد", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] ProposalFormRequiredDocumentMainGrid searchInput)
        {
            return Json(ProposalFormRequiredDocumentService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ProposalFormRequiredDocumentMainGrid searchInput)
        {
            var result = ProposalFormRequiredDocumentService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "لیست نوع فرم پیشنهاد", Icon = "fa-list-alt ")]
        [HttpGet]
        public ActionResult GetTypeList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(ProposalFormRequiredDocumentTypeService.GetSellect2List(searchInput));
        }
    }
}
