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
    [AreaConfig(ModualTitle = " پایه (ادمین)", Icon = "fa-archive", Title = "عوارض")]
    [CustomeAuthorizeFilter]
    public class DutyController : Controller
    {
        readonly IDutyManager DutyManager = null;
        public DutyController(
                IDutyManager DutyManager
            )
        {
            this.DutyManager = DutyManager;
        }

        [AreaConfig(Title = "عوارض", Icon = "fa-percentage", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "عوارض";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "Duty", new { area = "BaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست عوارض", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("BaseData", "Duty")));
        }

        [AreaConfig(Title = "افزودن عوارض جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateDutyVM input)
        {
            return Json(DutyManager.Create(input));
        }

        [AreaConfig(Title = "حذف عوارض", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(DutyManager.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک عوارض", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(DutyManager.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  عوارض", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateDutyVM input)
        {
            return Json(DutyManager.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست عوارض", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] DutyMainGrid searchInput)
        {
            return Json(DutyManager.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] DutyMainGrid searchInput)
        {
            var result = DutyManager.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
