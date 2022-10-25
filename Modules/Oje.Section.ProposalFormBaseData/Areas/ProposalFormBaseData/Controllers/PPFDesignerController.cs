using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.ProposalFormBaseData.Interfaces;
using ISiteSettingService = Oje.AccountService.Interfaces.ISiteSettingService;

namespace Oje.Section.ProposalFormBaseData.Areas.ProposalFormBaseData.Controllers
{
    [Area("ProposalFormBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات فرم پیشنهاد", Icon = "fa-file-powerpoint", Title = "طراحی فرم پیشنهاد")]
    [CustomeAuthorizeFilter]
    public class PPFDesignerController: Controller
    {
        readonly IProposalFormService ProposalFormService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public PPFDesignerController
            (
                IProposalFormService ProposalFormService,
                ISiteSettingService SiteSettingService
            )
        {
            this.ProposalFormService = ProposalFormService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "طراحی فرم پیشنهاد", Icon = "fa-palette", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "طراحی فرم پیشنهاد";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "PPFDesigner", new { area = "ProposalFormBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه طراحی فرم پیشنهاد", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("ProposalFormBaseData", "PPFDesigner")));
        }

        [AreaConfig(Title = "مشاهده لیست فرم های پیشنهاد", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetFormList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(ProposalFormService.GetSelect2List(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [HttpPost]
        [AreaConfig(Title = "مشاهده فرم پیشنهاد", Icon = "fa-list-alt")]
        public IActionResult GetFormJsonConfig(string fid)
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(ProposalFormService.GetJSonConfigFile(fid.ToIntReturnZiro(), SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
