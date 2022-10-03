using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.EmailService.Interfaces;
using Oje.EmailService.Models.View;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;

namespace Oje.Section.MessengerBaseData.Areas.MessengerBaseData.Controllers
{
    [Area("MessengerBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات پیام رسان", Icon = "fa-flag", Title = "صف ارسال ایمیل")]
    [CustomeAuthorizeFilter]
    public class EmailSendingQueueController: Controller
    {
        readonly IEmailSendingQueueService EmailSendingQueueService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public EmailSendingQueueController(
            IEmailSendingQueueService EmailSendingQueueService,
            ISiteSettingService SiteSettingService
            )
        {
            this.EmailSendingQueueService = EmailSendingQueueService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "صف ارسال ایمیل", Icon = "fa-paper-plane", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "صف ارسال ایمیل";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "EmailSendingQueue", new { area = "MessengerBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست صف ارسال ایمیل", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("MessengerBaseData", "EmailSendingQueue")));
        }

        [AreaConfig(Title = "مشاهده لیست صف ارسال ایمیل", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] EmailSendingQueueMainGrid searchInput)
        {
            return Json(EmailSendingQueueService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
