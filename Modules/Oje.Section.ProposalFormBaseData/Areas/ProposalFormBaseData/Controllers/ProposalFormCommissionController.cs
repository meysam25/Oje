using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.ProposalFormBaseData.Interfaces;
using Oje.Section.ProposalFormBaseData.Models.View;
using System;
using IProposalFormService = Oje.Section.ProposalFormBaseData.Interfaces.IProposalFormService;
using ISiteSettingService = Oje.AccountService.Interfaces.ISiteSettingService;

namespace Oje.Section.ProposalFormBaseData.Areas.ProposalFormBaseData.Controllers
{
    [Area("ProposalFormBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات فرم پیشنهاد", Order = 11, Icon = "fa-file-powerpoint", Title = "نرخ پورسانت")]
    [CustomeAuthorizeFilter]
    public class ProposalFormCommissionController : Controller
    {
        readonly IProposalFormCommissionService ProposalFormCommissionService = null;
        readonly IProposalFormService ProposalFormService = null;
        readonly ISiteSettingService SiteSettingService = null;

        public ProposalFormCommissionController
            (
                IProposalFormCommissionService ProposalFormCommissionService,
                IProposalFormService ProposalFormService,
                ISiteSettingService SiteSettingService
            )
        {
            this.ProposalFormCommissionService = ProposalFormCommissionService;
            this.ProposalFormService = ProposalFormService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "نرخ پورسانت", Icon = "fa-percentage", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "نرخ پورسانت";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ProposalFormCommission", new { area = "ProposalFormBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست نرخ پورسانت", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("ProposalFormBaseData", "ProposalFormCommission")));
        }

        [AreaConfig(Title = "افزودن نرخ پورسانت جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] ProposalFormCommissionCreateUpdateVM input)
        {
            return Json(ProposalFormCommissionService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف نرخ پورسانت", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(ProposalFormCommissionService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک نرخ پورسانت", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(ProposalFormCommissionService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  نرخ پورسانت", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] ProposalFormCommissionCreateUpdateVM input)
        {
            return Json(ProposalFormCommissionService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست نرخ پورسانت", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] ProposalFormCommissionMainGrid searchInput)
        {
            return Json(ProposalFormCommissionService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ProposalFormCommissionMainGrid searchInput)
        {
            var result = ProposalFormCommissionService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست فرم پیشنهاد ", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetProposalFormList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(ProposalFormService.GetSelect2List(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
