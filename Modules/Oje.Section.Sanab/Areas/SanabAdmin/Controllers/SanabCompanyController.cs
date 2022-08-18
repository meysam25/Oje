using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.Infrastructure;
using Oje.Infrastructure.Exceptions;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Sanab.Interfaces;
using Oje.Sanab.Models.View;
using System;

namespace Oje.Section.Sanab.Areas.SanabAdmin.Controllers
{
    [Area("SanabAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "سنحاب", Icon = "fa-cctv", Title = "شرکت بیمه")]
    [CustomeAuthorizeFilter]
    public class SanabCompanyController: Controller
    {
        readonly ISanabCompanyService SanabCompanyService = null;
        readonly ICompanyService CompanyService = null;

        public SanabCompanyController
            (
                ISanabCompanyService SanabCompanyService,
                ICompanyService CompanyService
            )
        {
            this.SanabCompanyService = SanabCompanyService;
            this.CompanyService = CompanyService;
        }

        [AreaConfig(Title = "شرکت بیمه", Icon = "fa-building", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "شرکت بیمه";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "SanabCompany", new { area = "SanabAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست شرکت بیمه", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("SanabAdmin", "SanabCompany")));
        }

        [AreaConfig(Title = "افزودن شرکت بیمه جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] SanabCompanyCreateUpdateVM input)
        {
            return Json(SanabCompanyService.Create(input));
        }

        [AreaConfig(Title = "حذف شرکت بیمه", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(SanabCompanyService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک شرکت بیمه", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(SanabCompanyService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  شرکت بیمه", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] SanabCompanyCreateUpdateVM input)
        {
            return Json(SanabCompanyService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست شرکت بیمه", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] SanabCompanyMainGrid searchInput)
        {
            return Json(SanabCompanyService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] SanabCompanyMainGrid searchInput)
        {
            var result = SanabCompanyService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست شرکت بیمه", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCompanyList()
        {
            return Json(CompanyService.GetLightList());
        }
    }
}
