using Oje.AccountManager.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.BaseData.Interfaces;
using Oje.Section.BaseData.Models.View;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.BaseData.Areas.BaseData.Controllers
{
    [Area("BaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = " پایه (ادمین)", Icon = "fa-archive", Title = "شرکت بیمه")]
    [CustomeAuthorizeFilter]
    public class CompanyController: Controller
    {
        readonly ICompanyManager CompanyManager = null;
        public CompanyController(ICompanyManager CompanyManager)
        {
            this.CompanyManager = CompanyManager;
        }

        [AreaConfig(Title = "شرکت بیمه", Icon = "fa-building", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "شرکت بیمه";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "Company", new { area = "BaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست شرکت بیمه", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("BaseData", "Company")));
        }

        [AreaConfig(Title = "افزودن شرکت بیمه جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateCompanyVM input)
        {
            return Json(CompanyManager.Create(input, HttpContext.GetLoginUserId()?.UserId));
        }

        [AreaConfig(Title = "حذف شرکت بیمه", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(CompanyManager.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک شرکت بیمه", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(CompanyManager.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  شرکت بیمه", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateCompanyVM input)
        {
            return Json(CompanyManager.Update(input, HttpContext.GetLoginUserId()?.UserId));
        }

        [AreaConfig(Title = "مشاهده لیست شرکت بیمه", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] CompanyMainGrid searchInput)
        {
            return Json(CompanyManager.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] CompanyMainGrid searchInput)
        {
            var result = CompanyManager.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
