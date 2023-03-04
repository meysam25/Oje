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
    [AreaConfig(ModualTitle = "هویتی", Icon = "fa-DashboardSectionCategorys", Title = "گروه بندی اطلاعات داشبورد")]
    [CustomeAuthorizeFilter]
    public class DashboardSectionCategoryController: Controller
    {
        readonly IDashboardSectionCategoryService DashboardSectionCategoryService = null;
        public DashboardSectionCategoryController(IDashboardSectionCategoryService DashboardSectionCategoryService)
        {
            this.DashboardSectionCategoryService = DashboardSectionCategoryService;
        }

        [AreaConfig(Title = "گروه بندی اطلاعات داشبورد", Icon = "fa-object-group", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "گروه بندی اطلاعات داشبورد";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "DashboardSectionCategory", new { area = "Account" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست گروه بندی اطلاعات داشبورد", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Account", "DashboardSectionCategory")));
        }

        [AreaConfig(Title = "افزودن گروه بندی اطلاعات داشبورد جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] DashboardSectionCategoryCreateUpdateVM input)
        {
            return Json(DashboardSectionCategoryService.Create(input));
        }

        [AreaConfig(Title = "حذف گروه بندی اطلاعات داشبورد", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(DashboardSectionCategoryService.Delete(input?.id));
        }

        [AreaConfig(Title = "مشاهده  یک گروه بندی اطلاعات داشبورد", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(DashboardSectionCategoryService.GetById(input?.id));
        }

        [AreaConfig(Title = "به روز رسانی  گروه بندی اطلاعات داشبورد", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] DashboardSectionCategoryCreateUpdateVM input)
        {
            return Json(DashboardSectionCategoryService.Update(input));
        }

        [AreaConfig(Title = "مشاهده لیست گروه بندی اطلاعات داشبورد", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] DashboardSectionCategoryServiceMainGrid searchInput)
        {
            return Json(DashboardSectionCategoryService.GetList(searchInput));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] DashboardSectionCategoryServiceMainGrid searchInput)
        {
            var result = DashboardSectionCategoryService.GetList(searchInput);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
