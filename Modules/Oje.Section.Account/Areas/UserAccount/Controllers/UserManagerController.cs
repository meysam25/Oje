using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using Oje.AccountService.Models.View;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.Account.Areas.UserAccount.Controllers
{
    [Area("UserAccount")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "حساب کاربری", Order = 1, Icon = "fa-users", Title = "کاربران")]
    [CustomeAuthorizeFilter]
    public class UserManagerController : Controller
    {
        readonly IUserService UserService = null;
        readonly IRoleService RoleService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly ICompanyService CompanyService = null;
        readonly PaymentService.Interfaces.IBankService BankService = null;

        public UserManagerController(
                IUserService UserService, 
                IRoleService RoleService, 
                ISiteSettingService SiteSettingService, 
                ICompanyService CompanyService,
                PaymentService.Interfaces.IBankService BankService
            )
        {
            this.UserService = UserService;
            this.RoleService = RoleService;
            this.SiteSettingService = SiteSettingService;
            this.CompanyService = CompanyService;
            this.BankService = BankService;
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
            return Json(UserService.CreateForUser(input, HttpContext.GetLoginUser()?.UserId, HttpContext.GetLoginUser(), SiteSettingService.GetSiteSetting()?.Id));
        }

        //[AreaConfig(Title = "افزودن کاربر جدید از جی سان", Icon = "fa-plus")]
        //[HttpPost]
        //public IActionResult CreateFromJson([FromForm] GlobalExcelFile input)
        //{
        //    return Json(UserService.CreateForUserFromJson(input, HttpContext.GetLoginUser()?.UserId, HttpContext.GetLoginUser(), SiteSettingService.GetSiteSetting()?.Id, SiteSettingService.GetSiteSetting()?.WebsiteUrl));
        //}

        [AreaConfig(Title = "حذف کاربر", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(UserService.DeleteForUser(input?.id, HttpContext.GetLoginUser(), SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک کاربر", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalLongId input)
        {
            return Json(UserService.GetByIdForUser(input?.id, HttpContext.GetLoginUser(), SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  کاربر", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateUserForUserVM input)
        {
            return Json(UserService.UpdateForUser(input, HttpContext.GetLoginUser()?.UserId, HttpContext.GetLoginUser(), SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست کاربران", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] UserServiceForUserMainGrid searchInput)
        {
            return Json(UserService.GetListForUser(searchInput, HttpContext.GetLoginUser(), SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] UserServiceForUserMainGrid searchInput)
        {
            var result = UserService.GetListForUser(searchInput, HttpContext.GetLoginUser(), SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست نقش ها", Icon = "fa-list-alt")]
        [HttpPost]
        public IActionResult GetRoleList()
        {
            return Json(RoleService.GetRoleLightListForUser(HttpContext.GetLoginUser(), SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست شرکت های بیمه", Icon = "fa-list-alt")]
        [HttpPost]
        public IActionResult GetCompanyList()
        {
            return Json(CompanyService.GetightList());
        }

        [AreaConfig(Title = "مشاهده لیست بانک ها", Icon = "fa-list-alt")]
        [HttpPost]
        public IActionResult GetBankList()
        {
            return Json(BankService.GetLightList());
        }
    }
}
