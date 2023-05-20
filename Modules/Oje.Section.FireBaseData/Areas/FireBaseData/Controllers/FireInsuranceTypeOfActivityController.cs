using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.FireBaseData.Interfaces;
using Oje.Section.FireBaseData.Models.View;
using Microsoft.AspNetCore.Mvc;
using System;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.FireBaseData.Areas.FireBaseData.Controllers
{
    [Area("FireBaseData")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = " پایه استعلام آتش سوزی", Icon = "fa-fire", Title = "نوع فعالیت")]
    [CustomeAuthorizeFilter]
    public class FireInsuranceTypeOfActivityController: Controller
    {
        readonly IFireInsuranceTypeOfActivityService FireInsuranceTypeOfActivityService = null;
        public FireInsuranceTypeOfActivityController(IFireInsuranceTypeOfActivityService FireInsuranceTypeOfActivityService)
        {
            this.FireInsuranceTypeOfActivityService = FireInsuranceTypeOfActivityService;
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
            return Json(FireInsuranceTypeOfActivityService.Create(input));
        }

        [AreaConfig(Title = "حذف نوع فعالیت", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(FireInsuranceTypeOfActivityService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک نوع فعالیت", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(FireInsuranceTypeOfActivityService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  نوع فعالیت", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] CreateUpdateFireInsuranceTypeOfActivityVM input)
        {
            return Json(FireInsuranceTypeOfActivityService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست نوع فعالیت", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] FireInsuranceTypeOfActivityMainGrid searchInput)
        {
            return Json(FireInsuranceTypeOfActivityService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] FireInsuranceTypeOfActivityMainGrid searchInput)
        {
            var result = FireInsuranceTypeOfActivityService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
