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
    [AreaConfig(ModualTitle = " پایه استعلام آتش سوزی (ادمین)", Icon = "fa-fire", Title = "نوع فعالیت")]
    [CustomeAuthorizeFilter]
    public class FireInsuranceTypeOfActivityController: Controller
    {
        readonly IFireInsuranceTypeOfActivityManager FireInsuranceTypeOfActivityManager = null;
        public FireInsuranceTypeOfActivityController(IFireInsuranceTypeOfActivityManager FireInsuranceTypeOfActivityManager)
        {
            this.FireInsuranceTypeOfActivityManager = FireInsuranceTypeOfActivityManager;
        }

        [AreaConfig(Title = "نوع فعالیت", Icon = "fa-user-md", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "نوع فعالیت";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "FireInsuranceTypeOfActivity", new { area = "FireBaseData" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست نوع فعالیت", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("FireBaseData", "FireInsuranceTypeOfActivity")));
        }

        [AreaConfig(Title = "افزودن نوع فعالیت جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] CreateUpdateFireInsuranceTypeOfActivityVM input)
        {
            return Json(FireInsuranceTypeOfActivityManager.Create(input));
        }

        [AreaConfig(Title = "حذف نوع فعالیت", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(FireInsuranceTypeOfActivityManager.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک نوع فعالیت", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(FireInsuranceTypeOfActivityManager.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  نوع فعالیت", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateFireInsuranceTypeOfActivityVM input)
        {
            return Json(FireInsuranceTypeOfActivityManager.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست نوع فعالیت", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] FireInsuranceTypeOfActivityMainGrid searchInput)
        {
            return Json(FireInsuranceTypeOfActivityManager.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] FireInsuranceTypeOfActivityMainGrid searchInput)
        {
            var result = FireInsuranceTypeOfActivityManager.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                return NotFound();
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                return NotFound();

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
