using Oje.AccountManager.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Financial.Interfaces;
using Oje.Section.Financial.Models.View;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Oje.Section.Financial.Areas.Financial.Controllers
{
    [Area("Financial")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "مالی", Icon = "fa-dollar-sign", Title = "لیست بانک")]
    [CustomeAuthorizeFilter]
    public class BankController: Controller
    {
        readonly IBankManager BankManager = null;
        public BankController(IBankManager BankManager)
        {
            this.BankManager = BankManager;
        }

        [AreaConfig(Title = "لیست بانک", Icon = "fa-university", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "لیست بانک";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "Bank", new { area = "Financial" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست لیست بانک", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Financial", "Bank")));
        }

        [AreaConfig(Title = "افزودن لیست بانک جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateBankVM input)
        {
            return Json(BankManager.Create(input, HttpContext.GetLoginUserId()?.UserId));
        }

        [AreaConfig(Title = "حذف لیست بانک", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(BankManager.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک لیست بانک", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(BankManager.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  لیست بانک", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateBankVM input)
        {
            return Json(BankManager.Update(input, HttpContext.GetLoginUserId()?.UserId));
        }

        [AreaConfig(Title = "مشاهده لیست لیست بانک", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] BankMainGrid searchInput)
        {
            return Json(BankManager.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] BankMainGrid searchInput)
        {
            var result = BankManager.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
