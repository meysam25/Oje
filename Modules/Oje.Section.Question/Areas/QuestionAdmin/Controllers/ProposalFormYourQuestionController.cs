using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Question.Interfaces;
using Oje.Section.Question.Models.View;
using System;

namespace Oje.Section.Question.Areas.QuestionAdmin.Controllers
{
    [Area("QuestionAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "سوالات", Icon = "fa-question", Title = "سوالات شما فرم پیشنهاد")]
    [CustomeAuthorizeFilter]
    public class ProposalFormYourQuestionController: Controller
    {
        readonly IProposalFormYourQuestionService ProposalFormYourQuestionService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly Interfaces.IProposalFormService ProposalFormService = null;

        public ProposalFormYourQuestionController
            (
                IProposalFormYourQuestionService ProposalFormYourQuestionService,
                ISiteSettingService SiteSettingService,
                Interfaces.IProposalFormService ProposalFormService
            )
        {
            this.ProposalFormYourQuestionService = ProposalFormYourQuestionService;
            this.SiteSettingService = SiteSettingService;
            this.ProposalFormService = ProposalFormService;
        }

        [AreaConfig(Title = "سوالات شما فرم پیشنهاد", Icon = "fa-book", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "سوالات شما فرم پیشنهاد";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ProposalFormYourQuestion", new { area = "QuestionAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست سوالات شما فرم پیشنهاد", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("QuestionAdmin", "ProposalFormYourQuestion")));
        }

        [AreaConfig(Title = "افزودن سوالات شما فرم پیشنهاد جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] ProposalFormYourQuestionCreateUpdateVM input)
        {
            return Json(ProposalFormYourQuestionService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف سوالات شما فرم پیشنهاد", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(ProposalFormYourQuestionService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک سوالات شما فرم پیشنهاد", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(ProposalFormYourQuestionService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  سوالات شما فرم پیشنهاد", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] ProposalFormYourQuestionCreateUpdateVM input)
        {
            return Json(ProposalFormYourQuestionService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست سوالات شما فرم پیشنهاد", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] ProposalFormYourQuestionMainGrid searchInput)
        {
            return Json(ProposalFormYourQuestionService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ProposalFormYourQuestionMainGrid searchInput)
        {
            var result = ProposalFormYourQuestionService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست فرم", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetProposalFormList([FromQuery] Select2SearchVM searchInput, [FromQuery] int? cSOWSiteSettingId)
        {
            return Json(ProposalFormService.GetSelect2List(searchInput, HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
