using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.AccountService.Models.View;
using Oje.Infrastructure;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using System;

namespace Oje.Section.Account.Areas.Account.Controllers
{
    [Area("Account")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "هویتی", Icon = "fa-users", Title = "روز های تعطیل")]
    [CustomeAuthorizeFilter]
    public class HolydayController: Controller
    {
        readonly IHolydayService HolydayService = null;
        public HolydayController
            (
                IHolydayService HolydayService
            )
        {
            this.HolydayService = HolydayService;
        }

        [AreaConfig(Title = "روز های تعطیل", Icon = "fa-calendar-day", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "روز های تعطیل";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "Holyday", new { area = "Account" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست روز های تعطیل", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Account", "Holyday")));
        }

        [AreaConfig(Title = "افزودن روز های تعطیل جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] HolydayCreateUpdateVM input)
        {
            return Json(HolydayService.Create(input));
        }

        [AreaConfig(Title = "حذف روز های تعطیل", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(HolydayService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک روز های تعطیل", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalLongId input)
        {
            return Json(HolydayService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  روز های تعطیل", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] HolydayCreateUpdateVM input)
        {
            return Json(HolydayService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست روز های تعطیل", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] HolydayMainGrid searchInput)
        {
            return Json(HolydayService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] HolydayMainGrid searchInput)
        {
            var result = HolydayService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
