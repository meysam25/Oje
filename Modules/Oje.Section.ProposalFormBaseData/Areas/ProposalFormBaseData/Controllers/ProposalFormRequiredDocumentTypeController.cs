using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.ProposalFormBaseData.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using Oje.Section.ProposalFormBaseData.Models.View;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.ProposalFormBaseData.Areas.ProposalFormBaseData.Controllers
{
    [Area("ProposalFormBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات فرم پیشنهاد", Icon = "fa-file-powerpoint", Title = "نوع مدارک مورد نیاز")]
    [CustomeAuthorizeFilter]
    public class ProposalFormRequiredDocumentTypeController: Controller
    {
        readonly IProposalFormRequiredDocumentTypeService ProposalFormRequiredDocumentTypeService = null;
        readonly IProposalFormService ProposalFormService = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;

        public ProposalFormRequiredDocumentTypeController(
            IProposalFormRequiredDocumentTypeService ProposalFormRequiredDocumentTypeService, 
            IProposalFormService ProposalFormService,
            AccountService.Interfaces.ISiteSettingService SiteSettingService
            )
        {
            this.ProposalFormRequiredDocumentTypeService = ProposalFormRequiredDocumentTypeService;
            this.ProposalFormService = ProposalFormService;
            this.SiteSettingService = SiteSettingService;
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
            return Json(ProposalFormRequiredDocumentTypeService.Create(input));
        }

        [AreaConfig(Title = "حذف نوع مدارک مورد نیاز", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(ProposalFormRequiredDocumentTypeService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک نوع مدارک مورد نیاز", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(ProposalFormRequiredDocumentTypeService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  نوع مدارک مورد نیاز", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateProposalFormRequiredDocumentTypeVM input)
        {
            return Json(ProposalFormRequiredDocumentTypeService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست نوع مدارک مورد نیاز", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] ProposalFormRequiredDocumentTypeMainGrid searchInput)
        {
            return Json(ProposalFormRequiredDocumentTypeService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ProposalFormRequiredDocumentTypeMainGrid searchInput)
        {
            var result = ProposalFormRequiredDocumentTypeService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست وب سایت ها", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetSiteList()
        {
            return Json(SiteSettingService.GetightList());
        }

        [AreaConfig(Title = "مشاهده لیست فرم های پیشنهاد", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetProposalFormList([FromQuery] Select2SearchVM searchInput, [FromQuery] int? cSOWSiteSettingId)
        {
            return Json(ProposalFormService.GetSelect2List(searchInput, HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
