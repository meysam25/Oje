using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Tender.Interfaces;
using Oje.Section.Tender.Models.View;
using System;
using Oje.AccountService.Interfaces;

namespace Oje.Section.Tender.Areas.TenderSuperAdmin.Controllers
{
    [Area("TenderSuperAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "پایه مناقصات", Icon = "fa-funnel-dollar", Title = "مدارک فرم")]
    [CustomeAuthorizeFilter]
    public class TenderProposalFormJsonConfigFileController: Controller
    {
        readonly ISiteSettingService SiteSettingService = null;
        readonly ITenderProposalFormJsonConfigFileService TenderProposalFormJsonConfigFileService = null;
        readonly ITenderProposalFormJsonConfigService TenderProposalFormJsonConfigService = null;
        public TenderProposalFormJsonConfigFileController (
            ISiteSettingService SiteSettingService,
            ITenderProposalFormJsonConfigFileService TenderProposalFormJsonConfigFileService,
            ITenderProposalFormJsonConfigService TenderProposalFormJsonConfigService
            )
        {
            this.SiteSettingService = SiteSettingService;
            this.TenderProposalFormJsonConfigFileService = TenderProposalFormJsonConfigFileService;
            this.TenderProposalFormJsonConfigService = TenderProposalFormJsonConfigService;
        }

        [AreaConfig(Title = "مدارک فرم", Icon = "fa-file", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "مدارک فرم";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "TenderProposalFormJsonConfigFile", new { area = "TenderSuperAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست مدارک فرم", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("TenderSuperAdmin", "TenderProposalFormJsonConfigFile")));
        }

        [AreaConfig(Title = "افزودن مدارک فرم جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] TenderProposalFormJsonConfigFileCreateUpdateVM input)
        {
            return Json(TenderProposalFormJsonConfigFileService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف مدارک فرم", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(TenderProposalFormJsonConfigFileService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده یک مدارک فرم", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(TenderProposalFormJsonConfigFileService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  مدارک فرم", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] TenderProposalFormJsonConfigFileCreateUpdateVM input)
        {
            return Json(TenderProposalFormJsonConfigFileService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست مدارک فرم", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] TenderProposalFormJsonConfigFileMainGrid searchInput)
        {
            return Json(TenderProposalFormJsonConfigFileService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] TenderProposalFormJsonConfigFileMainGrid searchInput)
        {
            var result = TenderProposalFormJsonConfigFileService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }


        [AreaConfig(Title = "مشاهده لیست فرم های ثبت نام", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetFormList()
        {
            return Json(TenderProposalFormJsonConfigService.GetLightList(SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
