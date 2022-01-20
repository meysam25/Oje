using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Services;
using Oje.Section.Blog.Interfaces;
using Oje.Section.Blog.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Oje.Section.Blog.Areas.Blog.Controllers
{
    [Area("Blog")]
    public class BlogController : Controller
    {
        readonly IBlogService BlogService = null;
        readonly IBlogCategoryService BlogCategoryService = null;
        readonly ISiteSettingService SiteSettingService = null;
        readonly IBlogTagService BlogTagService = null;
        public BlogController
            (
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

        [Route("[Area]/[Controller]/[Action]")]
        [HttpPost]
        public ActionResult GetAddReviewJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Blog", "AddBlogRviews")));
        }

        [Route("[Area]/[Controller]/[Action]")]
        [HttpPost]
        public ActionResult GetMainBlog()
        {
            return Json(BlogService.GetTopBlogs(4, SiteSettingService.GetSiteSetting()?.Id));
        }

        [Route("Blogs/Category/{catTitle}/{catId}")]
        [Route("Blogs/{KeywordTitle}/{keyWordId}")]
        [Route("Blogs")]
        [HttpGet]
        public ActionResult Blogs(string KeywordTitle, long? keyWordId, string catTitle, int? catId)
        {
            if (keyWordId.ToLongReturnZiro() > 0)
            {
                var foundTag = BlogTagService.GetBy(keyWordId.ToLongReturnZiro(), SiteSettingService.GetSiteSetting()?.Id);
                if (foundTag == null || foundTag.Title.Replace(" ", "-").Replace("--", "-") != KeywordTitle)
                    return NotFound();
                ViewBag.keyWordId = keyWordId;
                GlobalServices.FillSeoInfo(
                   ViewData,
                    "کلیه اخبار، مقاله‌ها و ویدیوهای مرتبط با " + foundTag.Title,
                    "اخرین اخبار و مقالات و ویدیو ها و پادکست های مرتبط با " + foundTag.Title,
                    Request.Scheme + "://" + Request.Host + "/Blogs/" + foundTag.Title.Trim().Replace(" ", "-").Replace("--", "-") + "/" + foundTag.Id,
                    Request.Scheme + "://" + Request.Host + "/Blogs" + foundTag.Title.Trim().Replace(" ", "-").Replace("--", "-") + "/" + foundTag.Id,
                    WebSiteTypes.website,
                    Request.Scheme + "://" + Request.Host + "/Modules/Assets/MainPage/logo.png",
                    null
                    );
            }
            else if (catId.ToIntReturnZiro() > 0)
            {
                var foundTag = BlogCategoryService.GetBy(catId.ToIntReturnZiro(), SiteSettingService.GetSiteSetting()?.Id);
                if (foundTag == null || foundTag.title.Replace(" ", "-").Replace("--", "-") != catTitle)
                    return NotFound();
                ViewBag.catId = catId;
                ViewData["metaDescription"] = "اخرین اخبار و مقالات و ویدیو ها و پادکست های مرتبط با " + foundTag.title;

                GlobalServices.FillSeoInfo(
                   ViewData,
                    "کلیه اخبار، مقاله‌ها و ویدیوهای مرتبط با " + foundTag.title,
                    "اخرین اخبار و مقالات و ویدیو ها و پادکست های مرتبط با " + foundTag.title,
                    Request.Scheme + "://" + Request.Host + "/Blogs",
                    Request.Scheme + "://" + Request.Host + "/Blogs",
                    WebSiteTypes.website,
                    Request.Scheme + "://" + Request.Host + "/Modules/Assets/MainPage/logo.png",
                    null
                    );
            }
            else
            {
                GlobalServices.FillSeoInfo(
                    ViewData,
                     "جدیدترین اخبار، مقاله‌ها و ویدیوهای ‌ستاد بیمه را در این صفحه دنبال کنید",
                     "آخرین اخبار گروه ستاد بیمه ، اطلاع‌رسانی، بیانیه مطبوعاتی، رویدادها، مسئولیت‌های اجتماعی و سایر فعالیت‌های روابط عمومی ستاد بیمه",
                     Request.Scheme + "://" + Request.Host + "/Blogs",
                     Request.Scheme + "://" + Request.Host + "/Blogs",
                     WebSiteTypes.website,
                     Request.Scheme + "://" + Request.Host + "/Modules/Assets/MainPage/logo.png",
                     null
                     );
            }

            return View();
        }

        [Route("Blog/{id}/{catTitle}/{title}")]
        [HttpGet]
        public ActionResult Blog(long id)
        {
            var foundBlog = BlogService.GetByIdForWeb(id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetIpAddress());
            if (foundBlog == null)
                return NotFound();

            BlogService.SetViewOrLike(id, HttpContext.GetIpAddress(), BlogLastLikeAndViewType.View);

            ViewBag.mViews = BlogService.GetMostTypeBlogs(SiteSettingService.GetSiteSetting()?.Id, 10, BlogLastLikeAndViewType.View, id);
            ViewBag.mLikeBlogs = BlogService.GetMostTypeBlogs(SiteSettingService.GetSiteSetting()?.Id, 10, BlogLastLikeAndViewType.Like, id);

            ViewBag.curDomain = Request.Scheme + "://" + Request.Host;

            GlobalServices.FillSeoInfo(
                  ViewData,
                   foundBlog.title,
                   foundBlog.summery,
                   Request.Scheme + "://" + Request.Host + foundBlog.url,
                   Request.Scheme + "://" + Request.Host + foundBlog.url,
                   WebSiteTypes.article,
                    Request.Scheme + "://" + Request.Host + foundBlog.mainImage_address,
                   foundBlog.publishDate.ToEnDate()
                   );

            return View(foundBlog);
        }

        [Route("Blog/{id}/{catTitle}/{title}")]
        [HttpPost]
        public ActionResult BlogActions(long id, [FromForm] BlogWebAction input)
        {
            return Json(BlogService.BlogActions(id, input, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetIpAddress()));
        }

        [Route("[Area]/[Controller]/[Action]")]
        [HttpPost]
        public ActionResult GetCategories()
        {
            return Json(BlogCategoryService.GetLightListForWeb(SiteSettingService.GetSiteSetting()?.Id));
        }

        [Route("[Area]/[Controller]/[Action]")]
        [HttpPost]
        public ActionResult Search([FromForm] BlogSearchVM input)
        {
            return Json(BlogService.Search(input, SiteSettingService.GetSiteSetting()?.Id, 10));
        }
    }
}
