using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.PaymentService.Interfaces;
using Oje.PaymentService.Models.View;

namespace Oje.Section.FinancialBaseData.Areas.FinancialBaseData.Controllers
{
    [Area("FinancialBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "اطلاعات پایه مالی", Icon = "fa-dollar-sign", Title = "حساب کاربری تی تک")]
    [CustomeAuthorizeFilter]
    public class TitakUserController: Controller
    {
        readonly ITitakUserService TitakUserService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public TitakUserController
            (
                ITitakUserService TitakUserService,
                ISiteSettingService SiteSettingService
            )
        {
            this.TitakUserService = TitakUserService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "حساب کاربری تی تک", Icon = "fa-user", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "حساب کاربری تی تک";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "TitakUser", new { area = "FinancialBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه حساب کاربری تی تک", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("FinancialBaseData", "TitakUser")));
        }

        [AreaConfig(Title = "افزودن / به روز رسانی حساب کاربری تی تک", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult CreateUpdate([FromForm] TitakUserCreateUpdateVM input)
        {
            return Json(TitakUserService.CreateUpdate(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  حساب کاربری تی تک", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult Get()
        {
            return Json(TitakUserService.GetBy(SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
