using Oje.AccountService.Filters;
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
    [AreaConfig(ModualTitle = " پایه (ادمین)", Icon = "fa-archive", Title = "استان ها")]
    [CustomeAuthorizeFilter]
    public class ProvinceController: Controller
    {
        readonly IProvinceService ProvinceService = null;
        public ProvinceController(
                IProvinceService ProvinceService
            )
        {
            this.ProvinceService = ProvinceService;
        }

        [AreaConfig(Title = "استان", Icon = "fa-university", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "استان";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "Province", new { area = "BaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست استان", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("BaseData", "Province")));
        }

        [AreaConfig(Title = "افزودن استان جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateProvinceVM input)
        {
            return Json(ProvinceService.Create(input));
        }

        [AreaConfig(Title = "حذف استان", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(ProvinceService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک استان", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(ProvinceService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  استان", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateProvinceVM input)
        {
            return Json(ProvinceService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست استان", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] ProvinceMainGrid searchInput)
        {
            return Json(ProvinceService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] ProvinceMainGrid searchInput)
        {
            var result = ProvinceService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
