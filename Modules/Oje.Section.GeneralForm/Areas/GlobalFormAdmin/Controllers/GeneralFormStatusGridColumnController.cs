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

namespace Oje.Section.GlobalForms.Areas.GlobalFormAdmin.Controllers
{
    [Area("GlobalFormAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "فرم های عمومی", Icon = "fa-file-powerpoint", Title = "ستون های اضافی")]
    [CustomeAuthorizeFilter]
    public class GeneralFormStatusGridColumnController: Controller
    {
        readonly IGeneralFormStatusGridColumnService GeneralFormStatusGridColumnService = null;
        readonly IGeneralFormService GeneralFormService = null;
        readonly IGeneralFormStatusService GeneralFormStatusService = null;
        readonly IGeneralFilledFormKeyService GeneralFilledFormKeyService = null;

        public GeneralFormStatusGridColumnController
            (
                IGeneralFormStatusGridColumnService GeneralFormStatusGridColumnService,
                IGeneralFormService GeneralFormService,
                IGeneralFormStatusService GeneralFormStatusService,
                IGeneralFilledFormKeyService GeneralFilledFormKeyService
            )
        {
            this.GeneralFormStatusGridColumnService = GeneralFormStatusGridColumnService;
            this.GeneralFormService = GeneralFormService;
            this.GeneralFormStatusService = GeneralFormStatusService;
            this.GeneralFilledFormKeyService = GeneralFilledFormKeyService;
        }

        [AreaConfig(Title = "ستون های اضافی", Icon = "fa-columns", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "ستون های اضافی";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "GeneralFormStatusGridColumn", new { area = "GlobalFormAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست ستون های اضافی", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("GlobalFormAdmin", "GeneralFormStatusGridColumn")));
        }

        [AreaConfig(Title = "افزودن ستون های اضافی جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] GeneralFormStatusGridColumnCreateUpdateVM input)
        {
            return Json(GeneralFormStatusGridColumnService.Create(input));
        }

        [AreaConfig(Title = "حذف ستون های اضافی", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(GeneralFormStatusGridColumnService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک ستون های اضافی", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(GeneralFormStatusGridColumnService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  ستون های اضافی", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] GeneralFormStatusGridColumnCreateUpdateVM input)
        {
            return Json(GeneralFormStatusGridColumnService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست ستون های اضافی", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] GeneralFormStatusGridColumnMainGrid searchInput)
        {
            return Json(GeneralFormStatusGridColumnService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] GeneralFormStatusGridColumnMainGrid searchInput)
        {
            var result = GeneralFormStatusGridColumnService.GetList(searchInput);
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

        [AreaConfig(Title = "مشاهده لیست وضعیت", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetStatusList([FromQuery] Select2SearchVM searchInput, [FromQuery] long? fid)
        {
            return Json(GeneralFormStatusService.GetSelect2List(searchInput, fid));
        }

        [AreaConfig(Title = "مشاهده لیست کلید", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetKeyList([FromQuery] Select2SearchVM searchInput, [FromQuery] long? fid)
        {
            return Json(GeneralFilledFormKeyService.GetSelect2List(searchInput, fid));
        }
    }
}
