using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
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
    [AreaConfig(ModualTitle = "سوالات", Icon = "fa-question", Title = "سوالات شما")]
    [CustomeAuthorizeFilter]
    public class YourQuestionController: Controller
    {
        readonly IYourQuestionService YourQuestionService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public YourQuestionController
            (
                IYourQuestionService YourQuestionService, 
                ISiteSettingService SiteSettingService
            )
        {
            this.YourQuestionService = YourQuestionService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "سوالات شما", Icon = "fa-book", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "سوالات شما";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "YourQuestion", new { area = "QuestionAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست سوالات شما", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("QuestionAdmin", "YourQuestion")));
        }

        [AreaConfig(Title = "افزودن سوالات شما جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] YourQuestionCreateUpdateVM input)
        {
            return Json(YourQuestionService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف سوالات شما", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(YourQuestionService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک سوالات شما", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(YourQuestionService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  سوالات شما", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] YourQuestionCreateUpdateVM input)
        {
            return Json(YourQuestionService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست سوالات شما", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] YourQuestionMainGrid searchInput)
        {
            return Json(YourQuestionService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] YourQuestionMainGrid searchInput)
        {
            var result = YourQuestionService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
