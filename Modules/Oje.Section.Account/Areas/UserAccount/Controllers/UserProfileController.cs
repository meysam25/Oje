using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.AccountService.Models.View;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Services;
using Oje.JoinServices.Interfaces;
using Oje.Security.Interfaces;

namespace Oje.Section.Account.Areas.UserAccount.Controllers
{
    [Area("UserAccount")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "حساب کاربری", Icon = "fa-users", Title = "به روز رسانی اطلاعات کاربر")]
    [CustomeAuthorizeFilter]
    public class UserProfileController: Controller
    {
        readonly IUserService UserService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IBlockAutoIpService BlockAutoIpService = null;
        readonly IUserNotifierService UserNotifierService = null;

        public UserProfileController
            (
                IUserService UserService,
                ISiteSettingService SiteSettingService,
                IBlockAutoIpService BlockAutoIpService,
                IUserNotifierService UserNotifierService
            )
        {
            this.SiteSettingService = SiteSettingService;
            this.UserService = UserService;
            this.BlockAutoIpService = BlockAutoIpService;
            this.UserNotifierService = UserNotifierService;
        }

        [AreaConfig(Title = "به روز رسانی اطلاعات کاربر", Icon = "fa-info", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "به روز رسانی اطلاعات کاربر";
            ViewBag.layer = "_WebLayout";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "UserProfile", new { area = "UserAccount" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه به روز رسانی اطلاعات کاربر", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("UserAccount", "UserProfile")));
        }

        [AreaConfig(Title = "مشاهده اطلاعات کاربر", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult Get()
        {
            return Json(UserService.GetBy(HttpContext.GetLoginUser()?.UserId, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی اطلاعات کاربر", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] UpdateUserForUserVM input)
        {
            BlockAutoIpService.CheckIfRequestIsValid(Infrastructure.Enums.BlockClientConfigType.UpdateUserProfile, Infrastructure.Enums.BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            var tempResult = UserService.UpdateUserProfile(input, HttpContext.GetLoginUser()?.UserId, SiteSettingService.GetSiteSetting()?.Id);
            BlockAutoIpService.CheckIfRequestIsValid(Infrastructure.Enums.BlockClientConfigType.UpdateUserProfile, Infrastructure.Enums.BlockAutoIpAction.BeforeExecute, HttpContext.GetIpAddress(), SiteSettingService.GetSiteSetting()?.Id);
            if(tempResult.isSuccess == true)
                UserNotifierService.Notify(HttpContext.GetLoginUser()?.UserId, Infrastructure.Enums.UserNotificationType.UserUpdateHisProfile, new System.Collections.Generic.List<Infrastructure.Models.PPFUserTypes>() { UserService.GetUserTypePPFInfo(HttpContext.GetLoginUser()?.UserId, Infrastructure.Enums.ProposalFilledFormUserType.OwnerUser) } , HttpContext.GetLoginUser()?.UserId, "به روز رسانی پروفایل کاربر", SiteSettingService.GetSiteSetting()?.Id, "/UserAccount/UserManager/Index" );

            return Json(tempResult);
        }
    }
}
