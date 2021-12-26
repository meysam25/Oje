using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using Oje.AccountService.Models.View;

namespace Oje.Section.Account.Areas.Account.Controllers
{
    [Area("Account")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = " هویتی (ادمین)", Icon = "fa-users", Title = "نقش ها")]
    [CustomeAuthorizeFilter]
    public class RoleManagerController : Controller
    {
        readonly IRoleService RoleService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IProposalFormService ProposalFormService = null;
        readonly IDashboardSectionService DashboardSectionService = null;
        readonly IActionService ActionService = null;
        public RoleManagerController(
                IRoleService RoleService,
                ISiteSettingService SiteSettingService,
                IProposalFormService ProposalFormService,
                IDashboardSectionService DashboardSectionService,
                IActionService ActionService
            )
        {
            this.RoleService = RoleService;
            this.SiteSettingService = SiteSettingService;
            this.ProposalFormService = ProposalFormService;
            this.DashboardSectionService = DashboardSectionService;
            this.ActionService = ActionService;
        }


        [AreaConfig(Title = "نقش", Icon = "fa-user-tag", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "نقش";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "RoleManager", new { area = "Account" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست نقش", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Account", "RoleManager")));
        }

        [AreaConfig(Title = "افزودن نقش جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateRoleVM input)
        {
            return Json(RoleService.Create(input));
        }

        [AreaConfig(Title = "حذف نقش", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(RoleService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک نقش", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(RoleService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  نقش", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateRoleVM input)
        {
            return Json(RoleService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست نقش", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] RoleGridFilters searchInput)
        {
            return Json(RoleService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] RoleGridFilters searchInput)
        {
            var result = RoleService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست تنظیمات")]
        [HttpPost]
        public IActionResult GetSettingList()
        {
            return Json(SiteSettingService.GetightList());
        }

        [AreaConfig(Title = "مشاهده لیست فرم های پیشنهاد")]
        [HttpGet]
        public IActionResult GetProposalFormList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(ProposalFormService.GetightListForSelect2(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }



        [AreaConfig(Title = "افزودن بخش نقش جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult CreateRoleAction([FromForm] DashboardSectionCreateUpdateVM input)
        {
            return Json(DashboardSectionService.Create(input));
        }

        [AreaConfig(Title = "حذف بخش نقش", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult DeleteRoleAction([FromForm] GlobalIntId input)
        {
            return Json(DashboardSectionService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک بخش نقش", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetRoleAction([FromForm] GlobalIntId input)
        {
            return Json(DashboardSectionService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی بخش نقش", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult UpdateRoleAction([FromForm] DashboardSectionCreateUpdateVM input)
        {
            return Json(DashboardSectionService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست بخش نقش", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetRoleActionList([FromForm] DashboardSectionGridFilters searchInput)
        {
            return Json(DashboardSectionService.GetList(searchInput));
        }

        [AreaConfig(Title = "مشاهده لیست بخش ها", Icon = "fa-list-alt")]
        [HttpGet]
        public IActionResult GetSettingList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(ActionService.GetightListForSelect2(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

    }
}
