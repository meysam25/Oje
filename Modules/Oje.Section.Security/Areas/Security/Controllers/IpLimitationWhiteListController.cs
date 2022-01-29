using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using Oje.Security.Interfaces;
using Oje.Security.Models.View;

namespace Oje.Section.Security.Areas.Security.Controllers
{
    [Area("Security")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "امنیت", Icon = "fa-user-secret", Title = "ای پی مجاز")]
    [CustomeAuthorizeFilter]
    public class IpLimitationWhiteListController: Controller
    {
        readonly IIpLimitationWhiteListService IpLimitationWhiteListService = null;
        public IpLimitationWhiteListController(
                IIpLimitationWhiteListService IpLimitationWhiteListService
            )
        {
            this.IpLimitationWhiteListService = IpLimitationWhiteListService;
        }

        [AreaConfig(Title = "ای پی مجاز", Icon = "fa-network-wired", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "ای پی مجاز";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "IpLimitationWhiteList", new { area = "Security" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست ای پی مجاز", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Security", "IpLimitationWhiteList")));
        }

        [AreaConfig(Title = "افزودن ای پی مجاز جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateIpLimitationWhiteListVM input)
        {
            return Json(IpLimitationWhiteListService.Create(input));
        }

        [AreaConfig(Title = "حذف ای پی مجاز", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(IpLimitationWhiteListService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک ای پی مجاز", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(IpLimitationWhiteListService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  ای پی مجاز", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateIpLimitationWhiteListVM input)
        {
            return Json(IpLimitationWhiteListService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست ای پی مجاز", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] IpLimitationWhiteListMainGrid searchInput)
        {
            return Json(IpLimitationWhiteListService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] IpLimitationWhiteListMainGrid searchInput)
        {
            var result = IpLimitationWhiteListService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
