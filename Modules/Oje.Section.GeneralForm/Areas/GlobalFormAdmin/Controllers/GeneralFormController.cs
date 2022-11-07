using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.GlobalForms.Interfaces;
using System;
using Oje.Section.GlobalForms.Models.View;

namespace Oje.Section.GlobalForms.Areas.GlobalFormAdmin.Controllers
{
    [Area("GlobalFormAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "فرم های عمومی", Icon = "fa-file-powerpoint", Title = "فرم عمومی")]
    [CustomeAuthorizeFilter]
    public class GeneralFormController : Controller
    {
        readonly IGeneralFormService GeneralFormService = null;
        readonly ISiteSettingService SiteSettingService = null;

        public GeneralFormController
            (
                IGeneralFormService GeneralFormService,
                ISiteSettingService SiteSettingService
            )
        {
            this.GeneralFormService = GeneralFormService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "فرم عمومی", Icon = "fa-file-alt", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "فرم عمومی";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "GeneralForm", new { area = "GlobalFormAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست فرم عمومی", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("GlobalFormAdmin", "GeneralForm")));
        }

        [AreaConfig(Title = "افزودن فرم عمومی جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] GeneralFormCreateUpdateVM input)
        {
            return Json(GeneralFormService.Create(input));
        }

        [AreaConfig(Title = "حذف فرم عمومی", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(GeneralFormService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک فرم عمومی", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalLongId input)
        {
            return Json(GeneralFormService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  فرم عمومی", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] GeneralFormCreateUpdateVM input)
        {
            return Json(GeneralFormService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست فرم عمومی", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] GeneralFormMainGrid searchInput)
        {
            return Json(GeneralFormService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] GeneralFormMainGrid searchInput)
        {
            var result = GeneralFormService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "لیست وب سایت ها", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetSiteList()
        {
            return Json(SiteSettingService.GetLightList());
        }
    }
}
