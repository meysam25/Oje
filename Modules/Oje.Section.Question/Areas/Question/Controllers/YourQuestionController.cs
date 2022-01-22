using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Interfaces;
using Oje.Section.Question.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Question.Areas.Question.Controllers
{
    [Area("Question")]
    [Route("[Area]/[Controller]/[Action]")]
    public class YourQuestionController: Controller
    {
        readonly IYourQuestionService YourQuestionService = null;
        readonly ISiteSettingService SiteSettingService = null;
        public YourQuestionController
            (
                IYourQuestionService YourQuestionService,
                ISiteSettingService SiteSettingService
            )
        {
            this.YourQuestionService = YourQuestionService;
            this.SiteSettingService = SiteSettingService;
        }

        public IActionResult GetList()
        {
            return Json(YourQuestionService.GetListForWeb(SiteSettingService.GetSiteSetting()?.Id));
        }
    }
}
