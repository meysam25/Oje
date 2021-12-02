using Oje.AccountManager.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.FireBaseData.Interfaces;
using Oje.Section.FireBaseData.Models.View;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.FireBaseData.Areas.FireBaseData.Controllers
{
    [Area("FireBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = " پایه استعلام آتش سوزی (ادمین)", Icon = "fa-fire", Title = "پوشش")]
    [CustomeAuthorizeFilter]
    public class FireInsuranceCoverageTitleController: Controller
    {
        readonly IFireInsuranceCoverageTitleManager FireInsuranceCoverageTitleManager = null;
        public FireInsuranceCoverageTitleController
            (
                IFireInsuranceCoverageTitleManager FireInsuranceCoverageTitleManager
            )
        {
            this.FireInsuranceCoverageTitleManager = FireInsuranceCoverageTitleManager;
        }

        [AreaConfig(Title = "پوشش", Icon = "fa-heading", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "پوشش";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "FireInsuranceCoverageTitle", new { area = "FireBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست پوشش", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("FireBaseData", "FireInsuranceCoverageTitle")));
        }

        [AreaConfig(Title = "افزودن پوشش جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateFireInsuranceCoverageTitleVM input)
        {
            return Json(FireInsuranceCoverageTitleManager.Create(input));
        }

        [AreaConfig(Title = "حذف پوشش", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(FireInsuranceCoverageTitleManager.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک پوشش", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(FireInsuranceCoverageTitleManager.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  پوشش", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateFireInsuranceCoverageTitleVM input)
        {
            return Json(FireInsuranceCoverageTitleManager.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست پوشش", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] FireInsuranceCoverageTitleMainGrid searchInput)
        {
            return Json(FireInsuranceCoverageTitleManager.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] FireInsuranceCoverageTitleMainGrid searchInput)
        {
            var result = FireInsuranceCoverageTitleManager.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
