using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Filters;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Filters;
using Oje.Infrastructure.Models;
using Oje.Infrastructure.Services;
using Oje.Section.Blog.Interfaces;
using Oje.Section.Blog.Models.View;
using System;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.Blog.Areas.BlogAdmin.Controllers
{
    [Area("BlogAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "بلاگ و خبرنامه", Order = 10, Icon = "fa-blog", Title = "گروه بندی")]
    [CustomeAuthorizeFilter]
    public class BlogCategoryController: Controller
    {
        readonly IBlogCategoryService BlogCategoryService = null;
        readonly ISiteSettingService SiteSettingService = null;

        public BlogCategoryController(
                IBlogCategoryService BlogCategoryService,
                ISiteSettingService SiteSettingService
            )
        {
            this.BlogCategoryService = BlogCategoryService;
            this.SiteSettingService = SiteSettingService;
        }

        [AreaConfig(Title = "گروه بندی", Icon = "fa-object-group", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "گروه بندی";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "BlogCategory", new { area = "BlogAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست گروه بندی", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("BlogAdmin", "BlogCategory")));
        }

        [AreaConfig(Title = "افزودن گروه بندی جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] BlogCategoryCreateUpdateVM input)
        {
            return Json(BlogCategoryService.Create(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف گروه بندی", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalIntId input)
        {
            return Json(BlogCategoryService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده  یک گروه بندی", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalIntId input)
        {
            return Json(BlogCategoryService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "به روز رسانی  گروه بندی", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] BlogCategoryCreateUpdateVM input)
        {
            return Json(BlogCategoryService.Update(input, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست گروه بندی", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] BlogCategoryMainGrid searchInput)
        {
            return Json(BlogCategoryService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] BlogCategoryMainGrid searchInput)
        {
            var result = BlogCategoryService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }
    }
}
