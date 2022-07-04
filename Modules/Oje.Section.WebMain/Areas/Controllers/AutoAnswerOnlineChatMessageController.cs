using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.WebMain.Interfaces;
using Oje.Section.WebMain.Models.View;
using System;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.WebMain.Areas.Controllers
{
    [Area("WebMainAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "وب سایت", Icon = "fa-browser", Title = "پاسخ های خودکار")]
    [CustomeAuthorizeFilter]
    public class AutoAnswerOnlineChatMessageController: Controller
    {
        readonly ISiteSettingService SiteSettingService = null;
        readonly IAutoAnswerOnlineChatMessageService AutoAnswerOnlineChatMessageService = null;

        public AutoAnswerOnlineChatMessageController
            (
                ISiteSettingService SiteSettingService,
                IAutoAnswerOnlineChatMessageService AutoAnswerOnlineChatMessageService
            )
        {
            this.AutoAnswerOnlineChatMessageService = AutoAnswerOnlineChatMessageService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "پاسخ های خودکار", Icon = "fa-comment", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "پاسخ های خودکار";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "AutoAnswerOnlineChatMessage", new { area = "WebMainAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست پاسخ های خودکار", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("WebMainAdmin", "AutoAnswerOnlineChatMessage")));
        }

        [AreaConfig(Title = "افزودن پاسخ های خودکار جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] AutoAnswerOnlineChatMessageCreateUpdateVM input)
        {
            return Json(AutoAnswerOnlineChatMessageService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف پاسخ های خودکار", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(AutoAnswerOnlineChatMessageService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک پاسخ های خودکار", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(AutoAnswerOnlineChatMessageService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  پاسخ های خودکار", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] AutoAnswerOnlineChatMessageCreateUpdateVM input)
        {
            return Json(AutoAnswerOnlineChatMessageService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست پاسخ های خودکار", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] AutoAnswerOnlineChatMessageMainGrid searchInput)
        {
            return Json(AutoAnswerOnlineChatMessageService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] AutoAnswerOnlineChatMessageMainGrid searchInput)
        {
            var result = AutoAnswerOnlineChatMessageService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
