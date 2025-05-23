﻿using Microsoft.AspNetCore.Mvc;
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
using IBlogService = Oje.Section.Blog.Interfaces.IBlogService;

namespace Oje.Section.Blog.Areas.BlogAdmin.Controllers
{
    [Area("BlogAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "بلاگ و خبرنامه", Order = 10, Icon = "fa-blog", Title = "بلاگ")]
    [CustomeAuthorizeFilter]
    public class BlogController: Controller
    {
        readonly IBlogService BlogService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IBlogCategoryService BlogCategoryService = null;
        readonly IBlogTagService BlogTagService = null;

        public BlogController(
                IBlogService BlogService,
                ISiteSettingService SiteSettingService,
                IBlogCategoryService BlogCategoryService,
                IBlogTagService BlogTagService
            )
        {
            this.BlogService = BlogService;
            this.SiteSettingService = SiteSettingService;
            this.BlogCategoryService = BlogCategoryService;
            this.BlogTagService = BlogTagService;
        }

        [AreaConfig(Title = "بلاگ", Icon = "fa-rss", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "بلاگ";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "Blog", new { area = "BlogAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست بلاگ", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("BlogAdmin", "Blog")));
        }

        [AreaConfig(Title = "افزودن بلاگ جدید", Icon = "fa-plus")]
        [HttpPost]
        public IActionResult Create([FromForm] BlogCreateUpdateVM input)
        {
            return Json(BlogService.Create(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser().UserId));
        }

        [AreaConfig(Title = "حذف بلاگ", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] GlobalLongId input)
        {
            return Json(BlogService.Delete(input?.id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده یک بلاگ", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] GlobalLongId input)
        {
            return Json(BlogService.GetById(input?.id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetIpAddress()));
        }

        [AreaConfig(Title = "به روز رسانی بلاگ", Icon = "fa-pencil")]
        [HttpPost]
        public IActionResult Update([FromForm] BlogCreateUpdateVM input)
        {
            return Json(BlogService.Update(input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser().UserId));
        }

        [AreaConfig(Title = "مشاهده لیست بلاگ", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetList([FromForm] BlogMainGrid searchInput)
        {
            return Json(BlogService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "خروجی اکسل", Icon = "fa-file-excel")]
        [HttpPost]
        public ActionResult Export([FromForm] BlogMainGrid searchInput)
        {
            var result = BlogService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id);
            if (result == null || result.data == null || result.data.Count == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);
            var byteResult = ExportToExcel.Export(result.data);
            if (byteResult == null || byteResult.Length == 0)
                throw BException.GenerateNewException(BMessages.Not_Found);

            return Json(Convert.ToBase64String(byteResult));
        }

        [AreaConfig(Title = "مشاهده لیست گروه بندی", Icon = "fa-list-alt")]
        [HttpPost]
        public ActionResult GetCatList([FromQuery] int? cSOWSiteSettingId)
        {
            return Json(BlogCategoryService.GetLightList(HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست تگ", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetTagList([FromQuery] Select2SearchVM searchInput, [FromQuery] int? cSOWSiteSettingId)
        {
            return Json(BlogTagService.GetSelect2List(searchInput, HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "مشاهده لیست بلاگ", Icon = "fa-list-alt")]
        [HttpGet]
        public ActionResult GetBlogLightist([FromQuery] Select2SearchVM searchInput, [FromQuery] int? cSOWSiteSettingId)
        {
            return Json(BlogService.GetSelect2List(searchInput, HttpContext?.GetLoginUser()?.canSeeOtherWebsites == true && cSOWSiteSettingId.ToIntReturnZiro() > 0 ? cSOWSiteSettingId : SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
