using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Filters;
using Oje.Section.RegisterForm.Models.View;

namespace Oje.Section.RegisterForm.Areas.RegisterFormAdmin.Controllers
{
    [Area("RegisterFormAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "ثبت نام کاربر", Icon = "fa-users", Title = "شماره کارت پرداخت کارت به کارت")]
    [CustomeAuthorizeFilter]
    public class UserFilledRegisterFormTargetBankCardNoController: Controller
    {
        readonly IPropertyService PropertyService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public UserFilledRegisterFormTargetBankCardNoController
            (
                IPropertyService PropertyService,
                ISiteSettingService SiteSettingService
            )
        {
            this.PropertyService = PropertyService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "شماره کارت پرداخت کارت به کارت", Icon = "fa-address-card", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "شماره کارت پرداخت کارت به کارت";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "UserFilledRegisterFormTargetBankCardNo", new { area = "RegisterFormAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه شماره کارت پرداخت کارت به کارت", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("RegisterFormAdmin", "UserFilledRegisterFormTargetBankCardNo")));
        }

        [AreaConfig(Title = "افزودن / به روز رسانی شماره کارت پرداخت کارت به کارت", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult CreateUpdate([FromForm] UserFilledRegisterFormTargetBankCardNoCreateUpdateVM input)
        {
            return Json(PropertyService.CreateUpdate(input, SiteSettingService.GetSiteSetting()?.Id, PropertyType.UserFilledRegisterFormTargetBankCardNo));
        }

        [AreaConfig(Title = "مشاهده  شماره کارت پرداخت کارت به کارت", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult Get()
        {
            return Json(PropertyService.GetBy<UserFilledRegisterFormTargetBankCardNoVM>(PropertyType.UserFilledRegisterFormTargetBankCardNo, SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
