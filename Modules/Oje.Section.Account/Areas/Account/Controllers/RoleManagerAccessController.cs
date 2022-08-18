using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Models.View;

namespace Oje.Section.Account.Areas.Account.Controllers
{
    [Area("Account")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = " هویتی (ادمین)", Icon = "fa-users", Title = "دسترسی ها")]
    [CustomeAuthorizeFilter]
    public class RoleManagerAccessController : Controller
    {
        readonly ISectionService SectionService = null;

        public RoleManagerAccessController(ISectionService SectionService)
        {
            this.SectionService = SectionService;
        }

        [AreaConfig(Title = "صفحه دسترسی نقش")]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "ویرایش دسترسی نقش";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "RoleManagerAccess", new { area = "Account" });
            return View("Index");
        }

        [AreaConfig(Title = "دریافت تنظیمات صفحه")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Account", "RoleManagerAccess")));
        }

        [AreaConfig(Title = "مشاهده دسترسی نقش ها")]
        [HttpPost]
        public IActionResult GetModaulsList([FromForm]GlobalIntId input)
        {
            return Json(SectionService.GetListForTreeView(input?.id));
        }

        [AreaConfig(Title = "ویرایش دسترسی نقش ها")]
        [HttpPost]
        [RequestFormLimits(ValueCountLimit = 20000)]
        public IActionResult Update([FromForm]CreateUpdateRoleAccessVM input)
        {
            return Json(SectionService.UpdateAccess(input));
        }

        [AreaConfig(Title = "به روز رسانی لیست مازول ها")]
        [HttpPost]
        public IActionResult UpdateModals()
        {
            SectionService.UpdateModuals();
            return Json(true);
        }

    }
}
