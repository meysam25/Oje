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

namespace Oje.Section.Account.Areas.Account.Controllers
{
    [Area("Account")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = " هویتی (ادمین)", Icon = "fa-users", Title = "دسترسی ها")]
    [CustomeAuthorizeFilter]
    public class RoleServiceAccessController: Controller
    {
        readonly ISectionService SectionService = null;
        public RoleServiceAccessController(ISectionService SectionService)
        {
            this.SectionService = SectionService;
        }

        [AreaConfig(Title = "صفحه دسترسی نقش")]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "ویرایش دسترسی نقش";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "RoleServiceAccess", new { area = "Account" });
            return View("Index");
        }

        [AreaConfig(Title = "دریافت تنظیمات صفحه")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Account", "RoleServiceAccess")));
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

    }
}
