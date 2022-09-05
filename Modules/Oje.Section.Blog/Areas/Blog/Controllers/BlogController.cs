using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Services;
using Oje.Section.Blog.Interfaces;
using Oje.Section.Blog.Models.View;
using Oje.Infrastructure.Exceptions;
using System.Collections.Generic;
using Oje.Infrastructure.Models;
using IBlogService = Oje.Section.Blog.Interfaces.IBlogService;

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

        [HttpGet]
        [Route("BlogSiteMap.xml", Order = int.MaxValue - 1000)]
        public ActionResult BlogSiteMap()
        {
            Response.ContentType = "application/xml; charset=utf-8";
            return Content(BlogService.GetBlogSiteMap(SiteSettingService.GetSiteSetting()?.Id, Request.Scheme + "://" + Request.Host));
        }

        [Route("[Area]/[Controller]/[Action]")]
        [HttpPost]
        public ActionResult GetAddReviewJsonConfig()
        {
            Response.ContentType = "application/json; charset=utf-8";
            return Content(System.IO.File.ReadAllText(GlobalConfig.GetJsonConfigFile("Blog", "AddBlogRviews")));
        }

        [Route("[Area]/[Controller]/[Action]")]
        [HttpGet]
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
            var curSetting = SiteSettingService.GetSiteSetting();
            string title;
            string description;
            if (keyWordId.ToLongReturnZiro() > 0)
            {
                var foundTag = BlogTagService.GetBy(keyWordId.ToLongReturnZiro(), SiteSettingService.GetSiteSetting()?.Id);
                if (foundTag == null || foundTag.Title.Replace(" ", "-").Replace("--", "-") != KeywordTitle)
                    throw BException.GenerateNewException(BMessages.Not_Found);
                ViewBag.keyWordId = keyWordId;
                title = "کلیه اخبار، مقاله‌ها و ویدیوهای مرتبط با " + foundTag.Title;
                description = "اخرین اخبار و مقالات و ویدیو ها و پادکست های مرتبط با " + foundTag.Title;
            }
            else if (catId.ToIntReturnZiro() > 0)
            {
                var foundTag = BlogCategoryService.GetBy(catId.ToIntReturnZiro(), SiteSettingService.GetSiteSetting()?.Id);
                if (foundTag == null || foundTag.title.Replace(" ", "-").Replace("--", "-") != catTitle)
                    throw BException.GenerateNewException(BMessages.Not_Found);
                ViewBag.catId = catId;
                ViewData["metaDescription"] = "اخرین اخبار و مقالات و ویدیو ها و پادکست های مرتبط با " + foundTag.title;

                title = "کلیه اخبار، مقاله‌ها و ویدیوهای مرتبط با " + foundTag.title;
                description = "اخرین اخبار و مقالات و ویدیو ها و پادکست های مرتبط با " + foundTag.title;
            }
            else
            {
                title = "جدیدترین اخبار، مقاله‌ها و ویدیوهای ‌" + curSetting.Title + " را در این صفحه دنبال کنید";
                description = "آخرین اخبار گروه ستاد بیمه ، اطلاع‌رسانی، بیانیه مطبوعاتی، رویدادها، مسئولیت‌های اجتماعی و سایر فعالیت‌های روابط عمومی " + curSetting.Title + "";
            }

            GlobalServices.FillSeoInfo(
                    ViewData,
                    title,
                     description,
                     Request.Scheme + "://" + Request.Host + "/Blogs",
                     Request.Scheme + "://" + Request.Host + "/Blogs",
                     WebSiteTypes.website,
                     Request.Scheme + "://" + Request.Host + "/Modules/Images/news.png",
                     null,
                     LdJsonService.GetAboutUsJSObject(
                          Request.Scheme + "://" + Request.Host + "/",
                          Request.Scheme + "://" + Request.Host + "/Modules/Images/news.png",
                          title,
                          description,
                          null,
                          "NewsMediaOrganization"
                          )
                     );

            return View();
        }

        [Route("Blog/{id}/{catTitle}/{title}")]
        [HttpGet]
        public ActionResult Blog(long id)
        {
            var curSetting = SiteSettingService.GetSiteSetting();
            var foundBlog = BlogService.GetByIdForWeb(id, SiteSettingService.GetSiteSetting()?.Id, HttpContext.GetIpAddress());
            if (foundBlog == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

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
                   foundBlog.publishDate.ToEnDate(),
                   LdJsonService.GetNews(foundBlog.title, ViewBag.curDomain + foundBlog.mainImage_address, foundBlog.createDateEn, foundBlog.publishDateEn, foundBlog.user, foundBlog.catTitle, ViewBag.curDomain + foundBlog.url, foundBlog.summery),
                   LdJsonService.GetBreadcrumb(new List<KeyValue>() 
                    { 
                       new KeyValue() { key = curSetting.Title, value = Request.Scheme + "://" + Request.Host } , 
                       new KeyValue() { key = "مقالات", value = Request.Scheme + "://" + Request.Host + "/Blogs" }, 
                       new KeyValue() { key = foundBlog.title, value = "" } })
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
