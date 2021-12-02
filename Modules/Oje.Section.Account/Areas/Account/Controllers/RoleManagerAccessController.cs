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
    [AreaConfig(ModualTitle = " هویتی (ادمین)", Icon = "fa-users", Title = "دسترسی ها")]
    [CustomeAuthorizeFilter]
    public class RoleManagerAccessController: Controller
    {
        readonly ISectionManager SectionManager = null;
        public RoleManagerAccessController(ISectionManager SectionManager)
        {
            this.SectionManager = SectionManager;
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
            return Json(SectionManager.GetListForTreeView(input?.id));
        }

        [AreaConfig(Title = "ویرایش دسترسی نقش ها")]
        [HttpPost]
        public IActionResult Update([FromForm]CreateUpdateRoleAccessVM input)
        {
            return Json(SectionManager.UpdateAccess(input));
        }

    }
}
