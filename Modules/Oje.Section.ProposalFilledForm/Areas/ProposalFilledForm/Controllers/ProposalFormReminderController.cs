using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.ProposalFormService.Interfaces;
using Oje.ProposalFormService.Models.View;
using System;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.ProposalFilledForm.Areas.ProposalFilledForm.Controllers
{
    [Area("ProposalFilledForm")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "مدیریت بیمه نامه ها", Order = 12,  Icon = "fa-file-powerpoint", Title = "یادآوری")]
    [CustomeAuthorizeFilter]
    public class ProposalFormReminderController : Controller
    {
        readonly IProposalFormReminderService ProposalFormReminderService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IProposalFormCategoryService ProposalFormCategoryService = null;
        readonly ProposalFormService.Interfaces.IProposalFormService ProposalFormService = null;
        public ProposalFormReminderController
            (
                IProposalFormReminderService ProposalFormReminderService,
                ISiteSettingService SiteSettingService,
                IProposalFormCategoryService ProposalFormCategoryService,
                ProposalFormService.Interfaces.IProposalFormService ProposalFormService
            )
        {
            this.SiteSettingService = SiteSettingService;
            this.ProposalFormReminderService = ProposalFormReminderService;
            this.ProposalFormCategoryService = ProposalFormCategoryService;
            this.ProposalFormService = ProposalFormService;
        }

        [AreaConfig(Title = "یادآوری", Icon = "fa-alarm-clock", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "یادآوری";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ProposalFormReminder", new { area = "ProposalFilledForm" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست یادآوری", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("ProposalFilledForm", "ProposalFormReminder")));
        }

        [AreaConfig(Title = "افزودن یادآوری جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] ReminderCreateVM input)
        {
            return Json(ProposalFormReminderService.Create(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetIpAddress(), HttpContext.GetLoginUser()?.UserId));
        }

        [AreaConfig(Title = "حذف یادآوری", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] long? id)
        {
            return Json(ProposalFormReminderService.Delete(id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده یک یادآوری", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] long? id)
        {
            return Json(ProposalFormReminderService.GetById(id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  یادآوری", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] ReminderCreateVM input)
        {
            return Json(ProposalFormReminderService.Update(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId));
        }

        [AreaConfig(Title = "مشاهده لیست یادآوری", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] ProposalFormReminderMainGrid searchInput)
        {
            return Json(ProposalFormReminderService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ProposalFormReminderMainGrid searchInput)
        {
            var result = ProposalFormReminderService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [HttpGet]
        [AreaConfig(Title = "مشاهده لیست گروه بندی فرم پیشنهاد", Icon = "fa-list-alt")]
        public ActionResult GetProposalFormCategoryList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(ProposalFormCategoryService.GetSelect2List(searchInput));
        }

        [HttpGet]
        [AreaConfig(Title = "مشاهده لیست فرم پیشنهاد", Icon = "fa-list-alt")]
        public ActionResult GetProposalFormList([FromQuery] Select2SearchVM searchInput, [FromQuery] int? appfCatId, [FromQuery] int? cSOWSiteSettingId)
        {
            return Json(ProposalFormService.GetSelect2List(searchInput, appfCatId, HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
