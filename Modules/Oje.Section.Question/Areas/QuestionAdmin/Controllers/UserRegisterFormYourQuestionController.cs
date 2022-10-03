using Microsoft.AspNetCore.Http;
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
    [AreaConfig(ModualTitle = "سوالات", Icon = "fa-question", Title = "سوالات شما ثبت نام کاربر")]
    [CustomeAuthorizeFilter]
    public class UserRegisterFormYourQuestionController: Controller
    {
        readonly IUserRegisterFormYourQuestionService UserRegisterFormYourQuestionService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IUserRegisterFormService UserRegisterFormService = null;

        public UserRegisterFormYourQuestionController
            (
                IUserRegisterFormYourQuestionService UserRegisterFormYourQuestionService,
                ISiteSettingService SiteSettingService,
                IUserRegisterFormService UserRegisterFormService
            )
        {
            this.UserRegisterFormYourQuestionService = UserRegisterFormYourQuestionService;
            this.SiteSettingService = SiteSettingService;
            this.UserRegisterFormService = UserRegisterFormService;
        }

        [AreaConfig(Title = "سوالات شما ثبت نام کاربر", Icon = "fa-book", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "سوالات شما ثبت نام کاربر";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "UserRegisterFormYourQuestion", new { area = "QuestionAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست سوالات شما ثبت نام کاربر", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("QuestionAdmin", "UserRegisterFormYourQuestion")));
        }

        [AreaConfig(Title = "افزودن سوالات شما ثبت نام کاربر جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] UserRegisterFormYourQuestionCreateUpdateVM input)
        {
            return Json(UserRegisterFormYourQuestionService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف سوالات شما ثبت نام کاربر", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(UserRegisterFormYourQuestionService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک سوالات شما ثبت نام کاربر", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(UserRegisterFormYourQuestionService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  سوالات شما ثبت نام کاربر", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] UserRegisterFormYourQuestionCreateUpdateVM input)
        {
            return Json(UserRegisterFormYourQuestionService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست سوالات شما ثبت نام کاربر", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] UserRegisterFormYourQuestionMainGrid searchInput)
        {
            return Json(UserRegisterFormYourQuestionService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] UserRegisterFormYourQuestionMainGrid searchInput)
        {
            var result = UserRegisterFormYourQuestionService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست فرم", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetFormList([FromQuery] int? cSOWSiteSettingId)
        {
            return Json(UserRegisterFormService.GetLightList(HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
