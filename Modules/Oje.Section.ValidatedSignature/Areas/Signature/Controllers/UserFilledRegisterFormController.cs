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
    [AreaConfig(ModualTitle = "اعتبارسنجی امضا", Icon = "fa-spider-black-widow", Title = "کاربران ثبت نام کرده")]
    [CustomeAuthorizeFilter]
    public class UserFilledRegisterFormController: Controller
    {
        readonly IUserFilledRegisterFormService UserFilledRegisterFormService = null;

        public UserFilledRegisterFormController
            (
                IUserFilledRegisterFormService UserFilledRegisterFormService
            )
        {
            this.UserFilledRegisterFormService = UserFilledRegisterFormService;
        }

        [AreaConfig(Title = "کاربران ثبت نام کرده", Icon = "fa-users", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "کاربران ثبت نام کرده";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "UserFilledRegisterForm", new { area = "Signature" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست کاربران ثبت نام کرده", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Signature", "UserFilledRegisterForm")));
        }

        [AreaConfig(Title = "مشاهده جزئیات تغییرات غیر معتبر", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] long? id)
        {
            return Json(UserFilledRegisterFormService.GetBy(id));
        }

        [AreaConfig(Title = "مشاهده لیست کاربران ثبت نام کرده", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] UserFilledRegisterFormMainGrid searchInput)
        {
            return Json(UserFilledRegisterFormService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] UserFilledRegisterFormMainGrid searchInput)
        {
            var result = UserFilledRegisterFormService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
