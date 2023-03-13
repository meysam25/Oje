using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Services;
using System;
using Oje.ValidatedSignature.Interfaces;
using Oje.ValidatedSignature.Models.View;

namespace Oje.Section.ValidatedSignature.Areas.Signature.Controllers
{
    [Area("Signature")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "اعتبارسنجی امضا", Icon = "fa-spider-black-widow", Title = "کاربر")]
    [CustomeAuthorizeFilter]
    public class UserController: Controller
    {
        readonly IUserService UserService = null;

        public UserController
            (
                IUserService UserService
            )
        {
            this.UserService = UserService;
        }

        [AreaConfig(Title = "کاربر", Icon = "fa-users", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "کاربر";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "User", new { area = "Signature" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست کاربر", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Signature", "User")));
        }

        [AreaConfig(Title = "مشاهده جزئیات تغییرات غیر معتبر", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] long? id)
        {
            return Json(UserService.GetBy(id));
        }

        [AreaConfig(Title = "مشاهده لیست کاربر", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] UserMainGrid searchInput)
        {
            return Json(UserService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] UserMainGrid searchInput)
        {
            var result = UserService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
