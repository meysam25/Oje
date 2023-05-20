using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.ProposalFormBaseData.Interfaces;
using Oje.Section.ProposalFormBaseData.Models.View;
using System;

namespace Oje.Section.ProposalFormBaseData.Areas.ProposalFormBaseData.Controllers
{
    [Area("ProposalFormBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات فرم پیشنهاد", Order = 11, Icon = "fa-file-powerpoint", Title = "کد معرف")]
    [CustomeAuthorizeFilter]
    public class AgentRefferController : Controller
    {
        readonly ICompanyService CompanyService = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;
        readonly IAgentRefferService AgentRefferService = null;

        public AgentRefferController
            (
                AccountService.Interfaces.ISiteSettingService SiteSettingService,
                ICompanyService CompanyService,
                IAgentRefferService AgentRefferService
            )
        {
            this.SiteSettingService = SiteSettingService;
            this.CompanyService = CompanyService;
            this.AgentRefferService = AgentRefferService;
        }

        [AreaConfig(Title = "کد معرف", Icon = "fa-barcode", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "کد معرف";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "AgentReffer", new { area = "ProposalFormBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست کد معرف", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("ProposalFormBaseData", "AgentReffer")));
        }

        [AreaConfig(Title = "افزودن کد معرف جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] AgentRefferCreateUpdateVM input)
        {
            return Json(AgentRefferService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف کد معرف", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(AgentRefferService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک کد معرف", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(AgentRefferService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  کد معرف", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] AgentRefferCreateUpdateVM input)
        {
            return Json(AgentRefferService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست کد معرف", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] AgentRefferMainGrid searchInput)
        {
            return Json(AgentRefferService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] AgentRefferMainGrid searchInput)
        {
            var result = AgentRefferService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست شرکت ", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCompanyList()
        {
            return Json(CompanyService.GetLightList());
        }
    }
}
