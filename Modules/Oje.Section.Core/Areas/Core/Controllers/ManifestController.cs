using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Interfaces;

namespace Oje.Section.Core.Areas.Core.Controllers
{
    public class ManifestController : Controller
    {
        readonly ISiteSettingService SiteSettingService = null;
        public ManifestController(ISiteSettingService SiteSettingService)
        {
            this.SiteSettingService = SiteSettingService;
        }

        [HttpGet]
        [Route("manifest.json", Order = int.MaxValue - 100)]
        public IActionResult GetManifestJson()
        {
            return Json(SiteSettingService.GetManifest());
        }
    }
}
