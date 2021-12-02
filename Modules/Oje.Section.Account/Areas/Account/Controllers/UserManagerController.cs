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

namespace Oje.Section.Account.Areas.Account.Controllers
{
    [Area("Account")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = " هویتی (ادمین)", Icon = "fa-users", Title = "کاربران")]
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
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "UserManager", new { area = "Account" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست کاربران", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Account", "UserManager")));
        }

        [AreaConfig(Title = "افزودن کاربر جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm]CreateUpdateUserVM input)
        {
            return Json(UserManager.Create(input, HttpContext.GetLoginUserId()?.UserId));
        }

        [AreaConfig(Title = "حذف کاربر", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(UserManager.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک کاربر", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalLongId input)
        {
            return Json(UserManager.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  کاربر", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateUserVM input)
        {
            return Json(UserManager.Update(input, HttpContext.GetLoginUserId()?.UserId));
        }

        [AreaConfig(Title = "مشاهده لیست کاربران", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] UserManagerMainGrid searchInput)
        {
            return Json(UserManager.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] UserManagerMainGrid searchInput)
        {
            var result = UserManager.GetList(searchInput);
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
            return Json(RoleManager.GetRoleLightList());
        }

        [AreaConfig(Title = "مشاهده لیست تنظیمات")]
        [HttpPost]
        public IActionResult GetSettingList()
        {
            return Json(SiteSettingManager.GetightList());
        }

        [AreaConfig(Title = "مشاهده لیست شرکت های بیمه")]
        [HttpPost]
        public IActionResult GetCompanyList()
        {
            return Json(CompanyManager.GetightList());
        }
    }
}
