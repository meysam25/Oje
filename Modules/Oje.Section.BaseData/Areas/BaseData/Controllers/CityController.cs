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
    [AreaConfig(ModualTitle = " پایه (ادمین)", Icon = "fa-archive", Title = "شهر")]
    [CustomeAuthorizeFilter]
    public class CityController: Controller
    {
        readonly ICityManager CityManager = null;
        readonly IProvinceManager ProvinceManager = null;
        public CityController(ICityManager CityManager, IProvinceManager ProvinceManager)
        {
            this.CityManager = CityManager;
            this.ProvinceManager = ProvinceManager;
        }

        [AreaConfig(Title = "شهر", Icon = "fa-city", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "شهر";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "City", new { area = "BaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست شهر", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("BaseData", "City")));
        }

        [AreaConfig(Title = "افزودن شهر جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateCityVM input)
        {
            return Json(CityManager.Create(input));
        }

        [AreaConfig(Title = "حذف شهر", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(CityManager.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک شهر", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(CityManager.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  شهر", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateCityVM input)
        {
            return Json(CityManager.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست شهر", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] CityMainGrid searchInput)
        {
            return Json(CityManager.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] CityMainGrid searchInput)
        {
            var result = CityManager.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست استان", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetProvinceList()
        {
            return Json(ProvinceManager.GetLightList());
        }
    }
}
