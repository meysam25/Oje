using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Interfaces;
using Oje.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
