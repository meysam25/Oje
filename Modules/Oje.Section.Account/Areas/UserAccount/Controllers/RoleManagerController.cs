using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using Oje.AccountService.Models.View;

namespace Oje.Section.Account.Areas.UserAccount.Controllers
{
    [Area("UserAccount")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "حساب کاربری", Order = 1, Icon = "fa-users", Title = "نقش ها")]
    [CustomeAuthorizeFilter]
    public class RoleManagerController : Controller
    {
        readonly IRoleService RoleService = null;
        readonly IUserService UserService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IProposalFormService ProposalFormService = null;
        public RoleManagerController(
                IRoleService RoleService, 
                IUserService UserService,
                ISiteSettingService SiteSettingService,
                IProposalFormService ProposalFormService
            )
        {
            this.RoleService = RoleService;
            this.UserService = UserService;
            this.SiteSettingService = SiteSettingService;
            this.ProposalFormService = ProposalFormService;
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
            return Json(RoleService.CreateUser(input, UserService.GetLoginUser(), SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف نقش", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(RoleService.DeleteUser(input?.id, UserService.GetLoginUser(), SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک نقش", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(RoleService.GetByIdUser(input?.id, UserService.GetLoginUser(), SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  نقش", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateUserRoleVM input)
        {
            return Json(RoleService.UpdateUser(input, UserService.GetLoginUser(), SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست نقش", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] RoleUserGridFilters searchInput)
        {
            return Json(RoleService.GetListUser(searchInput, UserService.GetLoginUser(), SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] RoleUserGridFilters searchInput)
        {
            var result = RoleService.GetListUser(searchInput, UserService.GetLoginUser(), SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست فرم های پیشنهاد")]
        [HttpGet]
        public IActionResult GetProposalFormList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(ProposalFormService.GetightListForSelect2(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
