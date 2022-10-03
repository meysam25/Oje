using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Sms.Interfaces;
using Oje.Sms.Models.View;

namespace Oje.Section.MessengerBaseData.Areas.MessengerBaseData.Controllers
{
    [Area("MessengerBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "تنظیمات پیام رسان", Icon = "fa-flag", Title = "صف ارسال پیامک")]
    [CustomeAuthorizeFilter]
    public class SmsSendingQueueController: Controller
    {
        readonly ISmsSendingQueueService SmsSendingQueueService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public SmsSendingQueueController(
            ISmsSendingQueueService SmsSendingQueueService,
            ISiteSettingService SiteSettingService
            )
        {
            this.SmsSendingQueueService = SmsSendingQueueService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "صف ارسال پیامک", Icon = "fa-paper-plane", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "صف ارسال پیامک";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "SmsSendingQueue", new { area = "MessengerBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست صف ارسال پیامک", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("MessengerBaseData", "SmsSendingQueue")));
        }

        [AreaConfig(Title = "مشاهده لیست صف ارسال پیامک", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] SmsSendingQueueMainGrid searchInput)
        {
            return Json(SmsSendingQueueService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
