using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.GlobalForms.Interfaces;
using Oje.Section.GlobalForms.Models.View;

namespace Oje.Section.GlobalForms.Areas.GlobalFormAdmin.Controllers
{
    [Area("GlobalFormAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "فرم های عمومی", Icon = "fa-file-powerpoint", Title = "فرم های عمومی ثبت شده")]
    [CustomeAuthorizeFilter]
    public class GeneralFilledFormController : Controller
    {
        readonly IGeneralFilledFormService GeneralFilledFormService = null;
        readonly IGeneralFormStatusService GeneralFormStatusService = null;
        readonly IGeneralFilledFormStatusService GeneralFilledFormStatusService = null;
        readonly AccountService.Interfaces.ISiteSettingService SiteSettingService = null;

        public GeneralFilledFormController
            (
                IGeneralFilledFormService GeneralFilledFormService,
                AccountService.Interfaces.ISiteSettingService SiteSettingService,
                IGeneralFormStatusService GeneralFormStatusService,
                IGeneralFilledFormStatusService GeneralFilledFormStatusService
            )
        {
            this.GeneralFilledFormService = GeneralFilledFormService;
            this.SiteSettingService = SiteSettingService;
            this.GeneralFormStatusService = GeneralFormStatusService;
            this.GeneralFilledFormStatusService = GeneralFilledFormStatusService;
        }

        [AreaConfig(Title = "فرم های عمومی ثبت شده", Icon = "fa-file-alt", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "فرم های عمومی ثبت شده";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "GeneralFilledForm", new { area = "GlobalFormAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست فرم های عمومی ثبت شده", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("GlobalFormAdmin", "GeneralFilledForm")));
        }

        [AreaConfig(Title = "حذف فرم های عمومی ثبت شده", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(GeneralFilledFormService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()));
        }

        [AreaConfig(Title = "مشاهده یک فرم های عمومی ثبت شده", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalLongId input)
        {
            ViewBag.makeLayoutNull = true;
            return View(GeneralFilledFormService.PdfDetailes(input != null ? input.id.Value : -1, SiteSettingService.GetSiteSetting()?.Id, null, HttpContext.GetLoginUser()));
        }

        [AreaConfig(Title = "مشاهده لیست فرم های عمومی ثبت شده", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] GeneralFilledFormMainGrid searchInput)
        {
            return Json(GeneralFilledFormService.GetList(searchInput, Request.Form, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()));
        }

        [AreaConfig(Title = "به روز رسانی وضعیت فرم", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult UpdateStatus([FromForm] GeneralFilledFormUpdateStatusVM input)
        {
            var curLoginUser = HttpContext.GetLoginUser();
            return Json(GeneralFilledFormService.UpdateStatus(input, SiteSettingService.GetSiteSetting()?.Id, curLoginUser?.UserId, curLoginUser?.roles));
        }

        [AreaConfig(Title = "مشاهده لیست فرم وضعیت عمومی ثبت شده", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetStatusLogList([FromForm] GeneralFilledFormStatusMainGrid searchInput)
        {
            return Json(GeneralFilledFormStatusService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()));
        }

        [AreaConfig(Title = "مشاهده لیست فرم وضعیت", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetFormStatusList()
        {
            return Json(GeneralFormStatusService.GetLightList(HttpContext.GetLoginUser()?.roles));
        }

        [AreaConfig(Title = "مشاهده اسناد فرم پیشنهاد صادر شده نماینده", Icon = "fa-eye")]
        [HttpPost]
        public ActionResult GetPPFImageList([FromForm] GlobalGridParentLong input)
        {
            return Json(GeneralFilledFormService.GetUploadImages(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()));
        }
    }
}
