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
    [AreaConfig(ModualTitle = "امنیت", Icon = "fa-user-secret", Title = "ایمیل دریافت کننده خطا")]
    [CustomeAuthorizeFilter]
    public class DebugEmailReceiverController: Controller
    {
        readonly IDebugEmailReceiverService DebugEmailReceiverService = null;

        public DebugEmailReceiverController
            (
                IDebugEmailReceiverService DebugEmailReceiverService
            )
        {
            this.DebugEmailReceiverService = DebugEmailReceiverService;
        }

        [AreaConfig(Title = "ایمیل دریافت کننده خطا", Icon = "fa-at", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "ایمیل دریافت کننده خطا";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "DebugEmailReceiver", new { area = "Security" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست ایمیل دریافت کننده خطا", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Security", "DebugEmailReceiver")));
        }

        [AreaConfig(Title = "افزودن ایمیل دریافت کننده خطا جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] DebugEmailReceiverCreateUpdateVM input)
        {
            return Json(DebugEmailReceiverService.Create(input));
        }

        [AreaConfig(Title = "حذف ایمیل دریافت کننده خطا", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(DebugEmailReceiverService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک ایمیل دریافت کننده خطا", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(DebugEmailReceiverService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  ایمیل دریافت کننده خطا", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] DebugEmailReceiverCreateUpdateVM input)
        {
            return Json(DebugEmailReceiverService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست ایمیل دریافت کننده خطا", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] DebugEmailReceiverMainGrid searchInput)
        {
            return Json(DebugEmailReceiverService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] DebugEmailReceiverMainGrid searchInput)
        {
            var result = DebugEmailReceiverService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
