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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Blog.Areas.BlogAdmin.Controllers
{
    [Area("BlogAdmin")]
    [Route("[Area]/[Controller]/[Action]")]
    [AreaConfig(ModualTitle = "بلاگ", Icon = "fa-blog", Title = "نظرات بلاگ")]
    [CustomeAuthorizeFilter]
    public class BlogReviewController: Controller
    {
        readonly IBlogReviewService BlogReviewService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public BlogReviewController
            (
                IBlogReviewService BlogReviewService,
                ISiteSettingService SiteSettingService
            )
        {
            this.SiteSettingService = SiteSettingService;
            this.BlogReviewService = BlogReviewService;
        }

        [AreaConfig(Title = "نظرات بلاگ", Icon = "fa-comments", IsMainMenuItem = true)]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "نظرات بلاگ";
            ViewBag.ConfigRoute = Url.Action("GetJsonConfig", "BlogReview", new { area = "BlogAdmin" });
            return View();
        }

        [AreaConfig(Title = "تنظیمات صفحه لیست نظرات بلاگ", Icon = "fa-cog")]
        [HttpPost]
        public IActionResult GetJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("BlogAdmin", "BlogReview")));
        }

        [AreaConfig(Title = "مشاهده  یک نظر بلاگ", Icon = "fa-eye")]
        [HttpPost]
        public IActionResult GetById([FromForm] string id)
        {
            return Json(BlogReviewService.GetById(id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "حذف نظرات بلاگ", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Delete([FromForm] string id)
        {
            return Json(BlogReviewService.Delete(id, SiteSettingService.GetSiteSetting()?.Id));
        }

        [AreaConfig(Title = "تایید نظرات بلاگ", Icon = "fa-trash-o")]
        [HttpPost]
        public IActionResult Confirm([FromForm] string id)
        {
            return Json(BlogReviewService.Confirm(id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetLoginUser()?.UserId));
        }

        [AreaConfig(Title = "مشاهده لیست نظرات بلاگ", Icon = "fa-list-alt ")]
        [HttpPost]
        public ActionResult GetList([FromForm] BlogReviewMainGrid searchInput)
        {
            return Json(BlogReviewService.GetList(searchInput, SiteSettingService.GetSiteSetting()?.Id));
        }

    }
}
