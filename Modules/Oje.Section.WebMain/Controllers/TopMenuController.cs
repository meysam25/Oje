using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Interfaces;
using Oje.Section.WebMain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Controllers
{
    public class TopMenuController: Controller
    {
        readonly ITopMenuService TopMenuService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public TopMenuController
            (
                ITopMenuService TopMenuService,
                ISiteSettingService SiteSettingService
            )
        {
            this.TopMenuService = TopMenuService;
            this.SiteSettingService = SiteSettingService;
        }

        [Route("[Controller]/[Action]")]
        [HttpPost]
        public ActionResult GetTopMenu()
        {
            return Json(TopMenuService.GetListForWeb(SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
