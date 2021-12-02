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
    [AreaConfig(ModualTitle = " پایه (ادمین)", Icon = "fa-archive", Title = "مالیات")]
    [CustomeAuthorizeFilter]
    public class TaxController: Controller
    {
        readonly ITaxManager TaxManager = null;
        public TaxController(
                ITaxManager TaxManager
            )
        {
            this.TaxManager = TaxManager;
        }

        [AreaConfig(Title = "مالیات", Icon = "fa-percentage", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "مالیات";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "Tax", new { area = "BaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست مالیات", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("BaseData", "Tax")));
        }

        [AreaConfig(Title = "افزودن مالیات جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateTaxVM input)
        {
            return Json(TaxManager.Create(input));
        }

        [AreaConfig(Title = "حذف مالیات", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(TaxManager.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک مالیات", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(TaxManager.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  مالیات", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateTaxVM input)
        {
            return Json(TaxManager.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست مالیات", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] TaxMainGrid searchInput)
        {
            return Json(TaxManager.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] TaxMainGrid searchInput)
        {
            var result = TaxManager.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
