using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Core.Areas.Core.Controllers
{
    public class JsServicesController: Controller
    {
        readonly ISiteSettingService SiteSettingService = null;
        public JsServicesController
            (
                ISiteSettingService SiteSettingService
            )
        {
            this.SiteSettingService = SiteSettingService;
        }

        [Route("serviceWorker.js")]
        public IActionResult GetMainService()
        {
            Response.ContentType = "text/javascript";
            Response.Headers["Cache-Control"] = new TimeSpan(365, 0, 0, 0).TotalSeconds.ToString("0");
            return Content(SiteSettingService.GetMainService());
        }
    }
}
