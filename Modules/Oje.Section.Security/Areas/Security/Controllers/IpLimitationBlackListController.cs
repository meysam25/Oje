using Oje.AccountManager.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using Oje.Section.Security.Interfaces;
using Oje.Section.Security.Models.View;

namespace Oje.Section.Security.Areas.Security.Controllers
{
    [Area("Security")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "امنیت", Icon = "fa-user-secret", Title = "ای پی غیر مجاز")]
    [CustomeAuthorizeFilter]
    public class IpLimitationBlackListController: Controller
    {
        readonly IIpLimitationBlackListManager IpLimitationBlackListManager = null;
        public IpLimitationBlackListController(
                IIpLimitationBlackListManager IpLimitationBlackListManager
            )
        {
            this.IpLimitationBlackListManager = IpLimitationBlackListManager;
        }

        [AreaConfig(Title = "ای پی غیر مجاز", Icon = "fa-network-wired", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "ای پی غیر مجاز";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "IpLimitationBlackList", new { area = "Security" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست ای پی غیر مجاز", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Security", "IpLimitationBlackList")));
        }

        [AreaConfig(Title = "افزودن ای پی غیر مجاز جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateIpLimitationBlackListVM input)
        {
            return Json(IpLimitationBlackListManager.Create(input));
        }

        [AreaConfig(Title = "حذف ای پی غیر مجاز", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(IpLimitationBlackListManager.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک ای پی غیر مجاز", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(IpLimitationBlackListManager.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  ای پی غیر مجاز", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateIpLimitationBlackListVM input)
        {
            return Json(IpLimitationBlackListManager.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست ای پی غیر مجاز", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] IpLimitationBlackListMainGrid searchInput)
        {
            return Json(IpLimitationBlackListManager.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] IpLimitationBlackListMainGrid searchInput)
        {
            var result = IpLimitationBlackListManager.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
