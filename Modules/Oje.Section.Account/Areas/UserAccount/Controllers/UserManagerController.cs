using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.AccountService.Models;
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
using Oje.AccountService.Models.View;

namespace Oje.Section.Account.Areas.UserAccount.Controllers
{
    [Area("UserAccount")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "حساب کاربری", Icon = "fa-users", Title = "کاربران")]
    [CustomeAuthorizeFilter]
    public class UserServiceController: Controller
    {
        readonly IUserService UserService = null;
        readonly IRoleService RoleService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly ICompanyService CompanyService = null;

        public UserServiceController(
                IUserService UserService, 
                IRoleService RoleService, 
                ISiteSettingService SiteSettingService, 
                ICompanyService CompanyService
            )
        {
            this.UserService = UserService;
            this.RoleService = RoleService;
            this.SiteSettingService = SiteSettingService;
            this.CompanyService = CompanyService;
        }

        [AreaConfig(Title = "کاربران", Icon = "fa-user", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "کاربران";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "UserService", new { area = "UserAccount" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست کاربران", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("UserAccount", "UserService")));
        }

        [AreaConfig(Title = "افزودن کاربر جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm]CreateUpdateUserForUserVM input)
        {
            return Json(UserService.CreateForUser(input, HttpContext.GetLoginUserId()?.UserId, HttpContext.GetLoginUserId(), SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف کاربر", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(UserService.DeleteForUser(input?.id, HttpContext.GetLoginUserId(), SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک کاربر", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalLongId input)
        {
            return Json(UserService.GetByIdForUser(input?.id, HttpContext.GetLoginUserId(), SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  کاربر", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateUserForUserVM input)
        {
            return Json(UserService.UpdateForUser(input, HttpContext.GetLoginUserId()?.UserId, HttpContext.GetLoginUserId(), SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست کاربران", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] UserServiceForUserMainGrid searchInput)
        {
            return Json(UserService.GetListForUser(searchInput, HttpContext.GetLoginUserId(), SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] UserServiceForUserMainGrid searchInput)
        {
            var result = UserService.GetListForUser(searchInput, HttpContext.GetLoginUserId(), SiteSettingService.GetSiteSetting()?.Id);
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
            return Json(RoleService.GetRoleLightListForUser(HttpContext.GetLoginUserId(), SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست شرکت های بیمه")]
        [HttpPost]
        public IActionResult GetCompanyList()
        {
            return Json(CompanyService.GetightList());
        }
    }
}
