using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Security.Interfaces;
using Oje.Security.Models.View;
using System;

namespace Oje.Section.Security.Areas.Security.Controllers
{
    [Area("Security")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "امنیت", Icon = "fa-user-secret", Title = "رنج ای پی مجاز")]
    [CustomeAuthorizeFilter]
    public class ValidRangeIpController: Controller
    {
        readonly IValidRangeIpService ValidRangeIpService = null;

        public ValidRangeIpController
            (
                IValidRangeIpService ValidRangeIpService
            )
        {
            this.ValidRangeIpService = ValidRangeIpService;
        }

        [AreaConfig(Title = "رنج ای پی مجاز", Icon = "fa-network-wired", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "رنج ای پی مجاز";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "ValidRangeIp", new { area = "Security" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست رنج ای پی مجاز", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Security", "ValidRangeIp")));
        }

        [AreaConfig(Title = "افزودن رنج ای پی مجاز جدید از روی فایل اکسل", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult CreateFromXcel([FromForm] GlobalExcelFile input)
        {
            return Json(ValidRangeIpService.CreateFromExcel(input));
        }

        [AreaConfig(Title = "افزودن رنج ای پی مجاز جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] ValidRangeIpCreateUpdateVM input)
        {
            return Json(ValidRangeIpService.Create(input));
        }

        [AreaConfig(Title = "حذف رنج ای پی مجاز", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(ValidRangeIpService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک رنج ای پی مجاز", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(ValidRangeIpService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  رنج ای پی مجاز", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] ValidRangeIpCreateUpdateVM input)
        {
            return Json(ValidRangeIpService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست رنج ای پی مجاز", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] ValidRangeIpMainGrid searchInput)
        {
            return Json(ValidRangeIpService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ValidRangeIpMainGrid searchInput)
        {
            var result = ValidRangeIpService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
