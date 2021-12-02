using Oje.AccountManager.Filters;
using Oje.AccountManager.Interfaces;
using Oje.AccountManager.Models;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using Oje.AccountManager.Models.View;

namespace Oje.Section.Account.Areas.UserAccount.Controllers
{
    [Area("UserAccount")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "حساب کاربری", Icon = "fa-users", Title = "نقش ها")]
    [CustomeAuthorizeFilter]
    public class RoleManagerController : Controller
    {
        readonly IRoleManager RoleManager = null;
        readonly IUserManager UserManager = null;
        readonly ISiteSettingManager SiteSettingManager = null;
        readonly IProposalFormManager ProposalFormManager = null;
        public RoleManagerController(
                IRoleManager RoleManager, 
                IUserManager UserManager,
                ISiteSettingManager SiteSettingManager,
                IProposalFormManager ProposalFormManager
            )
        {
            this.RoleManager = RoleManager;
            this.UserManager = UserManager;
            this.SiteSettingManager = SiteSettingManager;
            this.ProposalFormManager = ProposalFormManager;
        }


        [AreaConfig(Title = "نقش", Icon = "fa-user-tag", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "نقش";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "RoleManager", new { area = "UserAccount" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست نقش", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("UserAccount", "RoleManager")));
        }

        [AreaConfig(Title = "افزودن نقش جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateUserRoleVM input)
        {
            return Json(RoleManager.CreateUser(input, UserManager.GetLoginUser(), SiteSettingManager.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف نقش", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(RoleManager.DeleteUser(input?.id, UserManager.GetLoginUser(), SiteSettingManager.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک نقش", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(RoleManager.GetByIdUser(input?.id, UserManager.GetLoginUser(), SiteSettingManager.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  نقش", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateUserRoleVM input)
        {
            return Json(RoleManager.UpdateUser(input, UserManager.GetLoginUser(), SiteSettingManager.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست نقش", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] RoleUserGridFilters searchInput)
        {
            return Json(RoleManager.GetListUser(searchInput, UserManager.GetLoginUser(), SiteSettingManager.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] RoleUserGridFilters searchInput)
        {
            var result = RoleManager.GetListUser(searchInput, UserManager.GetLoginUser(), SiteSettingManager.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست فرم های پیشنهاد")]
        [HttpGet]
        public IActionResult GetProposalFormList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(ProposalFormManager.GetightListForSelect2(searchInput, SiteSettingManager.GetSiteSetting()?.Id));
        }
    }
}
