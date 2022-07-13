using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Interfaces;
using Oje.Section.Question.Interfaces;

namespace Oje.Section.Question.Areas.Question.Controllers
{
    [Area("Question")]
    [Route("[Area]/[Controller]/[Action]")]
    public class UserRegisterFormYourQuestionController : Controller
    {
        readonly IUserRegisterFormYourQuestionService UserRegisterFormYourQuestionService = null;
        readonly ISiteSettingService SiteSettingService = null;

        public UserRegisterFormYourQuestionController
            (
                IUserRegisterFormYourQuestionService UserRegisterFormYourQuestionService,
                ISiteSettingService SiteSettingService
            )
        {
            this.UserRegisterFormYourQuestionService = UserRegisterFormYourQuestionService;
            this.SiteSettingService = SiteSettingService;
        }

        [HttpPost]
        public IActionResult GetList(int? fid)
        {
            return Json(UserRegisterFormYourQuestionService.GetListForWeb(SiteSettingService.GetSiteSetting()?.Id, fid));
        }
    }
}
