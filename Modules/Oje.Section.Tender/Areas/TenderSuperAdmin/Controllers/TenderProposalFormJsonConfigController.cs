using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Tender.Interfaces;
using Oje.Section.Tender.Models.View;
using System;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.Tender.Areas.TenderSuperAdmin.Controllers
{
    [Area("TenderSuperAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "پایه مناقصات", Icon = "fa-funnel-dollar", Title = "تنظیمات فرم پیشنهاد")]
    [CustomeAuthorizeFilter]
    public class TenderProposalFormJsonConfigController : Controller
    {
        readonly ITenderProposalFormJsonConfigService TenderProposalFormJsonConfigService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly Interfaces.IProposalFormService ProposalFormService = null;

        public TenderProposalFormJsonConfigController
            (
                ITenderProposalFormJsonConfigService TenderProposalFormJsonConfigService,
                ISiteSettingService SiteSettingService,
                Interfaces.IProposalFormService ProposalFormService
            )
        {
            this.TenderProposalFormJsonConfigService = TenderProposalFormJsonConfigService;
            this.SiteSettingService = SiteSettingService;
            this.ProposalFormService = ProposalFormService;
        }

        [AreaConfig(Title = "تنظیمات فرم پیشنهاد", Icon = "fa-file-powerpoint", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "تنظیمات فرم پیشنهاد";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "TenderProposalFormJsonConfig", new { area = "TenderSuperAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست تنظیمات فرم پیشنهاد", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("TenderSuperAdmin", "TenderProposalFormJsonConfig")));
        }

        [AreaConfig(Title = "افزودن تنظیمات فرم پیشنهاد جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] TenderProposalFormJsonConfigCreateUpdateVM input)
        {
            return Json(TenderProposalFormJsonConfigService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف تنظیمات فرم پیشنهاد", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(TenderProposalFormJsonConfigService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده یک تنظیمات فرم پیشنهاد", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(TenderProposalFormJsonConfigService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  تنظیمات فرم پیشنهاد", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] TenderProposalFormJsonConfigCreateUpdateVM input)
        {
            return Json(TenderProposalFormJsonConfigService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست تنظیمات فرم پیشنهاد", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] TenderProposalFormJsonConfigMainGrid searchInput)
        {
            return Json(TenderProposalFormJsonConfigService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] TenderProposalFormJsonConfigMainGrid searchInput)
        {
            var result = TenderProposalFormJsonConfigService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست فرم های پیشنهاد", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetProposalFormList([FromQuery] Select2SearchVM searchInput, [FromQuery] int? cSOWSiteSettingId)
        {
            return Json(ProposalFormService.GetSelect2List(searchInput, HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
