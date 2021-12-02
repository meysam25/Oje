using Microsoft.AspNetCore.Mvc;
using Oje.AccountManager.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.FireBaseData.Interfaces;
using Oje.Section.FireBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.FireBaseData.Areas.FireBaseData.Controllers
{
    [Area("FireBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = " پایه استعلام آتش سوزی (ادمین)", Icon = "fa-fire", Title = "سن بنا")]
    [CustomeAuthorizeFilter]
    public class FireInsuranceBuildingAgeController: Controller
    {
        readonly IFireInsuranceBuildingAgeManager FireInsuranceBuildingAgeManager = null;
        public FireInsuranceBuildingAgeController(IFireInsuranceBuildingAgeManager FireInsuranceBuildingAgeManager)
        {
            this.FireInsuranceBuildingAgeManager = FireInsuranceBuildingAgeManager;
        }

        [AreaConfig(Title = "سن بنا", Icon = "fa-building", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "سن بنا";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "FireInsuranceBuildingAge", new { area = "FireBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست سن بنا", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("FireBaseData", "FireInsuranceBuildingAge")));
        }

        [AreaConfig(Title = "افزودن سن بنا جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateFireInsuranceBuildingAgeVM input)
        {
            return Json(FireInsuranceBuildingAgeManager.Create(input));
        }

        [AreaConfig(Title = "حذف سن بنا", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(FireInsuranceBuildingAgeManager.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده یک سن بنا", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(FireInsuranceBuildingAgeManager.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  سن بنا", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateFireInsuranceBuildingAgeVM input)
        {
            return Json(FireInsuranceBuildingAgeManager.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست سن بنا", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] FireInsuranceBuildingAgeMainGrid searchInput)
        {
            return Json(FireInsuranceBuildingAgeManager.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] FireInsuranceBuildingAgeMainGrid searchInput)
        {
            var result = FireInsuranceBuildingAgeManager.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
