using Oje.AccountManager.Filters;
using Oje.AccountManager.Interfaces;
using Oje.AccountManager.Models;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oje.AccountManager.Models.View;

namespace Oje.Section.Account.Areas.UserAccount.Controllers
{
    [Area("UserAccount")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "حساب کاربری", Icon = "fa-users", Title = "دسترسی نقش ها")]
    [CustomeAuthorizeFilter]
    public class RoleManagerAccessController: Controller
    {
        readonly ISectionManager SectionManager = null;
        readonly ISiteSettingManager SiteSettingManager = null;
        readonly IUserManager UserManager = null;
        public RoleManagerAccessController(
                ISectionManager SectionManager, 
                ISiteSettingManager SiteSettingManager,
                IUserManager UserManager
            )
        {
            this.SectionManager = SectionManager;
            this.SiteSettingManager = SiteSettingManager;
            this.UserManager = UserManager;
        }

        [AreaConfig(Title = "صفحه دسترسی نقش")]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "ویرایش دسترسی نقش";
            ViewBag.ConfigRoute = Url.Action("GetAccessJsonConfig", "RoleManagerAccess", new { area = "UserAccount" });
            return View("Index");
        }

        [AreaConfig(Title = "دریافت تنظیمات صفحه")]
        [HttpPost]
        public IActionResult GetAccessJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("UserAccount", "RoleManagerAccess")));
        }

        [AreaConfig(Title = "مشاهده دسترسی نقش ها")]
        [HttpPost]
        public IActionResult GetModaulsList([FromForm]GlobalIntId input)
        {
            return Json(SectionManager.GetListForTreeViewForUser(input?.id, UserManager.GetLoginUser(), SiteSettingManager.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "ویرایش دسترسی نقش ها")]
        [HttpPost]
        public IActionResult Update([FromForm]CreateUpdateRoleAccessVM input)
        {
            return Json(SectionManager.UpdateAccessForUser(input, UserManager.GetLoginUser(), SiteSettingManager.GetSiteSetting()?.Id));
        }

       
    }
}
