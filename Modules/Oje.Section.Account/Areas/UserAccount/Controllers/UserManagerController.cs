using Oje.AccountManager.Filters;
using Oje.AccountManager.Interfaces;
using Oje.AccountManager.Models;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
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
    [AreaConfig(ModualTitle = "حساب کاربری", Icon = "fa-users", Title = "کاربران")]
    [CustomeAuthorizeFilter]
    public class UserManagerController: Controller
    {
        readonly IUserManager UserManager = null;
        readonly IRoleManager RoleManager = null;
        readonly ISiteSettingManager SiteSettingManager = null;
        readonly ICompanyManager CompanyManager = null;

        public UserManagerController(
                IUserManager UserManager, 
                IRoleManager RoleManager, 
                ISiteSettingManager SiteSettingManager, 
                ICompanyManager CompanyManager
            )
        {
            this.UserManager = UserManager;
            this.RoleManager = RoleManager;
            this.SiteSettingManager = SiteSettingManager;
            this.CompanyManager = CompanyManager;
        }

        [AreaConfig(Title = "کاربران", Icon = "fa-user", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "کاربران";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "UserManager", new { area = "UserAccount" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست کاربران", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("UserAccount", "UserManager")));
        }

        [AreaConfig(Title = "افزودن کاربر جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm]CreateUpdateUserForUserVM input)
        {
            return Json(UserManager.CreateForUser(input, HttpContext.GetLoginUserId()?.UserId, HttpContext.GetLoginUserId(), SiteSettingManager.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف کاربر", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(UserManager.DeleteForUser(input?.id, HttpContext.GetLoginUserId(), SiteSettingManager.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک کاربر", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalLongId input)
        {
            return Json(UserManager.GetByIdForUser(input?.id, HttpContext.GetLoginUserId(), SiteSettingManager.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  کاربر", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateUserForUserVM input)
        {
            return Json(UserManager.UpdateForUser(input, HttpContext.GetLoginUserId()?.UserId, HttpContext.GetLoginUserId(), SiteSettingManager.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست کاربران", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] UserManagerForUserMainGrid searchInput)
        {
            return Json(UserManager.GetListForUser(searchInput, HttpContext.GetLoginUserId(), SiteSettingManager.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] UserManagerForUserMainGrid searchInput)
        {
            var result = UserManager.GetListForUser(searchInput, HttpContext.GetLoginUserId(), SiteSettingManager.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست نقش ها")]
        [HttpPost]
        public IActionResult GetRoleList()
        {
            return Json(RoleManager.GetRoleLightListForUser(HttpContext.GetLoginUserId(), SiteSettingManager.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست شرکت های بیمه")]
        [HttpPost]
        public IActionResult GetCompanyList()
        {
            return Json(CompanyManager.GetightList());
        }
    }
}
