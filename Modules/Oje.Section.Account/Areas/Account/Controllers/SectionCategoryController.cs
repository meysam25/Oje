using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.AccountService.Models.View;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using System;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.Account.Areas.Account.Controllers
{
    [Area("Account")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = " هویتی (ادمین)", Icon = "fa-users", Title = "گروه بندی بخش منو")]
    [CustomeAuthorizeFilter]
    public class SectionCategoryController : Controller
    {
        readonly ISectionCategoryService SectionCategoryService = null;
        readonly ISectionService SectionService = null;
        public SectionCategoryController(ISectionCategoryService SectionCategoryService, ISectionService SectionService)
        {
            this.SectionCategoryService = SectionCategoryService;
            this.SectionService = SectionService;
        }

        [AreaConfig(Title = "گروه بندی بخش منو", Icon = "fa-user", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "گروه بندی بخش منو";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "SectionCategory", new { area = "Account" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست گروه بندی بخش منو", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Account", "SectionCategory")));
        }

        [AreaConfig(Title = "افزودن گروه بندی بخش منو جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] SectionCategoryCreateUpdateVM input)
        {
            return Json(SectionCategoryService.Create(input));
        }

        [AreaConfig(Title = "حذف گروه بندی بخش منو", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(SectionCategoryService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده گروه بندی بخش منو", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(SectionCategoryService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی گروه بندی بخش منو", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] SectionCategoryCreateUpdateVM input)
        {
            return Json(SectionCategoryService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست گروه بندی بخش منو", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] SectionCategoryMainGrid searchInput)
        {
            return Json(SectionCategoryService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] SectionCategoryMainGrid searchInput)
        {
            var result = SectionCategoryService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [HttpGet]
        [AreaConfig(Title = "لیست بخش ها", Icon = "fa-list-alt")]
        public IActionResult GetSectionList([FromQuery] Select2SearchVM searchInput)
        {
            return Json(SectionService.GetSelect2List(searchInput));
        }
    }
}
