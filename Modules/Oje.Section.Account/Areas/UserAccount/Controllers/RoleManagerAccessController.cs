using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Models.View;

namespace Oje.Section.Account.Areas.UserAccount.Controllers
{
    [Area("UserAccount")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "حساب کاربری", Icon = "fa-users", Title = "دسترسی نقش ها")]
    [CustomeAuthorizeFilter]
    public class RoleManagerAccessController : Controller
    {
        readonly ISectionService SectionService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IUserService UserService = null;
        public RoleManagerAccessController(
                ISectionService SectionService, 
                ISiteSettingService SiteSettingService,
                IUserService UserService
            )
        {
            this.SectionService = SectionService;
            this.SiteSettingService = SiteSettingService;
            this.UserService = UserService;
        }

        [AreaConfig(Title = "صفحه دسترسی نقش")]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "ویرایش دسترسی نقش";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "RoleManagerAccess", new { area = "UserAccount" });
            return View("Index");
        }

        [AreaConfig(Title = "دریافت تنظیمات صفحه")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("UserAccount", "RoleManagerAccess")));
        }

        [AreaConfig(Title = "مشاهده دسترسی نقش ها")]
        [HttpPost]
        public IActionResult GetModaulsList([FromForm]GlobalIntId input)
        {
            return Json(SectionService.GetListForTreeViewForUser(input?.id, UserService.GetLoginUser(), SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "ویرایش دسترسی نقش ها")]
        [HttpPost]
        public IActionResult Update([FromForm]CreateUpdateRoleAccessVM input)
        {
            return Json(SectionService.UpdateAccessForUser(input, UserService.GetLoginUser(), SiteSettingService.GetSiteSetting()?.Id));
        }

       
    }
}
