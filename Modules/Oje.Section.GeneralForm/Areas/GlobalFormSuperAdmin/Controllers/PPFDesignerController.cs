using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.GlobalForms.Interfaces;
using ISiteSettingService = Oje.AccountService.Interfaces.ISiteSettingService;

namespace Oje.Section.GlobalForms.Areas.GlobalFormSuperAdmin.Controllers
{
    [Area("GlobalFormSuperAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "پایه فرم های عمومی", Icon = "fa-file-powerpoint", Title = "طراحی فرم")]
    [CustomeAuthorizeFilter]
    public class PPFDesignerController : Controller
    {
        readonly ISiteSettingService SiteSettingService = null;
        readonly IGeneralFormService GeneralFormService = null;
        public PPFDesignerController
            (
                IGeneralFormService GeneralFormService,
                ISiteSettingService SiteSettingService
            )
        {
            this.GeneralFormService = GeneralFormService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "طراحی فرم", Icon = "fa-palette", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "طراحی فرم";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "PPFDesigner", new { area = "GlobalFormSuperAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه طراحی فرم", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("GlobalFormSuperAdmin", "PPFDesigner")));
        }

        [AreaConfig(Title = "مشاهده لیست فرم های", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetFormList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(GeneralFormService.GetSelect2List(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [HttpPost]
        [AreaConfig(Title = "مشاهده فرم", Icon = "fa-list-alt")]
        public IActionResult GetFormJsonConfig(string fid)
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(GeneralFormService.GetJSonConfigFile(fid.ToIntReturnZiro(), SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
