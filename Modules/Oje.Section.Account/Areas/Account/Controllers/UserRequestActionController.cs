using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.AccountService.Models.View;
using Oje.Infrastructure;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Services;
using System;

namespace Oje.Section.Account.Areas.Account.Controllers
{
    [Area("Account")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "هویتی", Icon = "fa-users", Title = "درخواست دسترسی کاربران")]
    [CustomeAuthorizeFilter]
    public class UserRequestActionController: Controller
    {
        readonly IUserService UserService = null;

        public UserRequestActionController(IUserService UserService)
        {
            this.UserService = UserService;
        }

        [AreaConfig(Title = "درخواست دسترسی کاربران", Icon = "fa-user-unlock", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "درخواست دسترسی کاربران";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "UserRequestAction", new { area = "Account" });
            return View("Index");
        }

        [AreaConfig(Title = "دریافت تنظیمات درخواست دسترسی کاربران")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Account", "UserRequestAction")));
        }

        [AreaConfig(Title = "حذف درخواست دسترسی کاربران", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] string id)
        {
            return Json(UserService.DeleteUserActionRequest(id));
        }

        [AreaConfig(Title = "تایید درخواست دسترسی کاربران", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Confirm([FromForm] string id)
        {
            return Json(UserService.ConfirmUserActionRequest(id));
        }

        [AreaConfig(Title = "مشاهده لیست درخواست دسترسی کاربران", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] UserRequestActionMainGrid searchInput)
        {
            return Json(UserService.GetRequestUserAccessList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] UserRequestActionMainGrid searchInput)
        {
            var result = UserService.GetRequestUserAccessList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
