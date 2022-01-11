using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Interfaces;
using Oje.Section.Blog.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Oje.Section.Blog.Areas.Blog.Controllers
{
    [Area("Blog")]
    public class BlogController: Controller
    {
        readonly IBlogService BlogService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public BlogController
            (
                IBlogService BlogService,
                ISiteSettingService SiteSettingService
            )
        {
            this.BlogService = BlogService;
            this.SiteSettingService = SiteSettingService;
        }

        [Route("[Area]/[Controller]/[Action]")]
        public ActionResult GetMainBlog()
        {
            return Json(BlogService.GetTopBlogs(4, SiteSettingService.GetSiteSetting()?.Id));
        }

        [Route("Blogs")]
        public ActionResult Blogs()
        {
            return View();
        }
    }
}
