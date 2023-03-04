using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.GlobalForms.Interfaces;
using Oje.Section.GlobalForms.Models.View;
using System;

namespace Oje.Section.GlobalForms.Areas.GlobalFormSuperAdmin.Controllers
{
    [Area("GlobalFormSuperAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "پایه فرم های عمومی", Icon = "fa-file-powerpoint", Title = "مدارک مورد نیاز فرم عمومی")]
    [CustomeAuthorizeFilter]
    public class GeneralFormRequiredDocumentController : Controller
    {
        readonly IGeneralFormRequiredDocumentService GeneralFormRequiredDocumentService = null;
        readonly IGeneralFormService GeneralFormService = null;

        public GeneralFormRequiredDocumentController
            (
                IGeneralFormRequiredDocumentService GeneralFormRequiredDocumentService,
                IGeneralFormService GeneralFormService
            )
        {
            this.GeneralFormRequiredDocumentService = GeneralFormRequiredDocumentService;
            this.GeneralFormService = GeneralFormService;
        }

        [AreaConfig(Title = "مدارک مورد نیاز فرم عمومی", Icon = "fa-file-image", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "مدارک مورد نیاز فرم عمومی";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "GeneralFormRequiredDocument", new { area = "GlobalFormSuperAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست مدارک مورد نیاز فرم عمومی", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("GlobalFormSuperAdmin", "GeneralFormRequiredDocument")));
        }

        [AreaConfig(Title = "افزودن مدارک مورد نیاز فرم عمومی جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] GeneralFormRequiredDocumentCreateUpdateVM input)
        {
            return Json(GeneralFormRequiredDocumentService.Create(input));
        }

        [AreaConfig(Title = "حذف مدارک مورد نیاز فرم عمومی", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(GeneralFormRequiredDocumentService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک مدارک مورد نیاز فرم عمومی", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalLongId input)
        {
            return Json(GeneralFormRequiredDocumentService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  مدارک مورد نیاز فرم عمومی", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] GeneralFormRequiredDocumentCreateUpdateVM input)
        {
            return Json(GeneralFormRequiredDocumentService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست مدارک مورد نیاز فرم عمومی", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] GeneralFormRequiredDocumentMainGrid searchInput)
        {
            return Json(GeneralFormRequiredDocumentService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] GeneralFormRequiredDocumentMainGrid searchInput)
        {
            var result = GeneralFormRequiredDocumentService.GetList(searchInput);
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
