using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.GlobalForms.Models.View;
using System;
using Oje.Section.GlobalForms.Interfaces;

namespace Oje.Section.GlobalForms.Areas.GlobalFormSuperAdmin.Controllers
{
    [Area("GlobalFormSuperAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "پایه فرم های عمومی", Icon = "fa-file-powerpoint", Title = "وضعیت فرم عمومی")]
    [CustomeAuthorizeFilter]
    public class GeneralFormStatusController : Controller
    {
        readonly IGeneralFormStatusService GeneralFormStatusService = null;
        readonly IGeneralFormService GeneralFormService = null;

        public GeneralFormStatusController
            (
                IGeneralFormStatusService GeneralFormStatusService,
                IGeneralFormService GeneralFormService
            )
        {
            this.GeneralFormStatusService = GeneralFormStatusService;
            this.GeneralFormService = GeneralFormService;
        }

        [AreaConfig(Title = "وضعیت فرم عمومی", Icon = "fa-object-group", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "وضعیت فرم عمومی";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "GeneralFormStatus", new { area = "GlobalFormSuperAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست وضعیت فرم عمومی", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("GlobalFormSuperAdmin", "GeneralFormStatus")));
        }

        [AreaConfig(Title = "افزودن وضعیت فرم عمومی جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] GeneralFormStatusCreateUpdateVM input)
        {
            return Json(GeneralFormStatusService.Create(input));
        }

        [AreaConfig(Title = "حذف وضعیت فرم عمومی", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(GeneralFormStatusService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک وضعیت فرم عمومی", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalLongId input)
        {
            return Json(GeneralFormStatusService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  وضعیت فرم عمومی", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] GeneralFormStatusCreateUpdateVM input)
        {
            return Json(GeneralFormStatusService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست وضعیت فرم عمومی", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] GeneralFormStatusMainGrid searchInput)
        {
            return Json(GeneralFormStatusService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] GeneralFormStatusMainGrid searchInput)
        {
            var result = GeneralFormStatusService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست فرم", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetFormList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(GeneralFormService.GetSelect2List(searchInput));
        }
    }
}
