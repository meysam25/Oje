using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Security.Interfaces;
using Oje.Security.Models.View;

namespace Oje.Section.Security.Areas.Security.Controllers
{
    [Area("Security")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "امنیت", Icon = "fa-user-secret", Title = "ایمیل ارسال کننده خطا")]
    [CustomeAuthorizeFilter]
    public class DebugEmailController: Controller
    {
        readonly IDebugEmailService DebugEmailService = null;

        public DebugEmailController
            (
                IDebugEmailService DebugEmailService
            )
        {
            this.DebugEmailService = DebugEmailService;
        }

        [AreaConfig(Title = "ایمیل ارسال کننده خطا", Icon = "fa-at", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "ایمیل ارسال کننده خطا";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "DebugEmail", new { area = "Security" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات ایمیل ارسال کننده خطا", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Security", "DebugEmail")));
        }

        [AreaConfig(Title = "افزودن / به روز رسانی ایمیل ارسال کننده خطا", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult CreateUpdate([FromForm] DebugEmailCreateUpdateVM input)
        {
            return Json(DebugEmailService.CreateUpdate(input));
        }

        [AreaConfig(Title = "مشاهده ایمیل ارسال کننده خطا", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult Get()
        {
            return Json(DebugEmailService.Get());
        }
    }
}
