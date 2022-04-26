using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Services;
using Oje.Section.WebMain.Interfaces;

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

        [Route("Page/{pid}/{pTitle}")]
        [HttpGet]
        public IActionResult Index(long? pid, string pTitle)
        {
            var foundPage = PageService.GetBy(pid, pTitle, SiteSettingService.GetSiteSetting()?.Id);
            if (foundPage == null)
                return NotFound();

            GlobalServices.FillSeoInfo(
                ViewData,
                 foundPage.title,
                 foundPage.summery,
                 Request.Scheme + "://" + Request.Host + foundPage.url,
                 Request.Scheme + "://" + Request.Host + foundPage.url,
                 WebSiteTypes.website,
                  Request.Scheme + "://" + Request.Host + foundPage.mainImageSmall,
                 null
                 );

            return View(foundPage);
        }
    }
}
