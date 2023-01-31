using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Services;
using Oje.Section.WebMain.Interfaces;
using Oje.Infrastructure.Exceptions;

namespace Oje.Section.WebMain.Controllers
{
    public class PageController: Controller
    {
        readonly IPageService PageService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public PageController
            (
                IPageService PageService,
                ISiteSettingService SiteSettingService
            )
        {
            this.PageService = PageService;
            this.SiteSettingService = SiteSettingService;
        }

        [HttpGet]
        [Route("SiteMap.xml", Order = int.MaxValue - 1000)]
        public ActionResult SiteMap()
        {
            Response.ContentType = "application/xml; charset=utf-8";
            return Content(PageService.GetSiteMap(SiteSettingService.GetSiteSetting()?.Id, Request.Scheme + "://" + Request.Host));
        }

        [Route("Page/{pid}/{pTitle}")]
        [HttpGet]
        public IActionResult Index(long? pid, string pTitle)
        {
            var foundPage = PageService.GetBy(pid, pTitle, SiteSettingService.GetSiteSetting()?.Id);
            if (foundPage == null)
                throw BException.GenerateNewException(BMessages.Not_Found);

            var curDomain = Request.Scheme + "://" + Request.Host;

            GlobalServices.FillSeoInfo(
                ViewData,
                 foundPage.title,
                 foundPage.summery,
                 Request.Scheme + "://" + Request.Host + foundPage.url,
                 Request.Scheme + "://" + Request.Host + foundPage.url,
                 WebSiteTypes.website,
                 Request.Scheme + "://" + Request.Host + foundPage.mainImageSmall,
                 null,
                 LdJsonService.GetNews2(foundPage.title, curDomain + foundPage.mainImage, curDomain + foundPage.mainImageSmall, curDomain + foundPage.url, foundPage.summery, foundPage.createDate)
                 );

            if (Request.IsMobile())
                return Json(foundPage);

            return View(foundPage);
        }
    }
}
