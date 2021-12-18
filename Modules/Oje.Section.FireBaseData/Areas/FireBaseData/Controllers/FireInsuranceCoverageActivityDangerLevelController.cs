using Oje.AccountService.Filters;
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
    [AreaConfig(ModualTitle = " پایه استعلام آتش سوزی (ادمین)", Icon = "fa-fire", Title = "نرخ پوشش های فعالیت")]
    [CustomeAuthorizeFilter]
    public class FireInsuranceCoverageActivityDangerLevelController: Controller
    {
        readonly IFireInsuranceCoverageActivityDangerLevelService FireInsuranceCoverageActivityDangerLevelService = null;
        readonly IFireInsuranceCoverageTitleService FireInsuranceCoverageTitleService = null;
        public FireInsuranceCoverageActivityDangerLevelController(
            IFireInsuranceCoverageActivityDangerLevelService FireInsuranceCoverageActivityDangerLevelService,
            IFireInsuranceCoverageTitleService FireInsuranceCoverageTitleService
            )
        {
            this.FireInsuranceCoverageActivityDangerLevelService = FireInsuranceCoverageActivityDangerLevelService;
            this.FireInsuranceCoverageTitleService = FireInsuranceCoverageTitleService;
        }

        [AreaConfig(Title = "نرخ پوشش های فعالیت", Icon = "fa-comments-dollar", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "نرخ پوشش های فعالیت";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "FireInsuranceCoverageActivityDangerLevel", new { area = "FireBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست نرخ پوشش های فعالیت", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("FireBaseData", "FireInsuranceCoverageActivityDangerLevel")));
        }

        [AreaConfig(Title = "افزودن نرخ پوشش های فعالیت جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateFireInsuranceCoverageActivityDangerLevelVM input)
        {
            return Json(FireInsuranceCoverageActivityDangerLevelService.Create(input));
        }

        [AreaConfig(Title = "حذف نرخ پوشش های فعالیت", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(FireInsuranceCoverageActivityDangerLevelService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک نرخ پوشش های فعالیت", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(FireInsuranceCoverageActivityDangerLevelService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  نرخ پوشش های فعالیت", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateFireInsuranceCoverageActivityDangerLevelVM input)
        {
            return Json(FireInsuranceCoverageActivityDangerLevelService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست نرخ پوشش های فعالیت", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] FireInsuranceCoverageActivityDangerLevelMainGrid searchInput)
        {
            return Json(FireInsuranceCoverageActivityDangerLevelService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] FireInsuranceCoverageActivityDangerLevelMainGrid searchInput)
        {
            var result = FireInsuranceCoverageActivityDangerLevelService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست پوشش ها", Icon = "fa-list-alt ")]
        [HttpGet]
        public ActionResult GetCoverTitleList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(FireInsuranceCoverageTitleService.GetSelect2List(searchInput));
        }
    }
}
