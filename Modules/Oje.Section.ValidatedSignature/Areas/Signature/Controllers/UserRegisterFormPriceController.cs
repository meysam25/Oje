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
    [AreaConfig(ModualTitle = "اعتبارسنجی امضا", Icon = "fa-spider-black-widow", Title = "مبلغ ثبت نام")]
    [CustomeAuthorizeFilter]
    public class UserRegisterFormPriceController: Controller
    {
        readonly IUserRegisterFormPriceService UserRegisterFormPriceService = null;

        public UserRegisterFormPriceController
            (
                IUserRegisterFormPriceService UserRegisterFormPriceService
            )
        {
            this.UserRegisterFormPriceService = UserRegisterFormPriceService;
        }

        [AreaConfig(Title = "مبلغ ثبت نام", Icon = "fa-usd-circle", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "مبلغ ثبت نام";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "UserRegisterFormPrice", new { area = "Signature" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست مبلغ ثبت نام", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Signature", "UserRegisterFormPrice")));
        }

        [AreaConfig(Title = "مشاهده جزئیات تغییرات غیر معتبر", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] long? id)
        {
            return Json(UserRegisterFormPriceService.GetBy(id));
        }

        [AreaConfig(Title = "مشاهده لیست مبلغ ثبت نام", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] UserRegisterFormPriceMainGrid searchInput)
        {
            return Json(UserRegisterFormPriceService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] UserRegisterFormPriceMainGrid searchInput)
        {
            var result = UserRegisterFormPriceService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
