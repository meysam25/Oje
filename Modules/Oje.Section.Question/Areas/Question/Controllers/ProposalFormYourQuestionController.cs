using Microsoft.AspNetCore.Mvc;
using Oje.AccountService.Interfaces;
using Oje.Section.Question.Interfaces;

namespace Oje.Section.Question.Areas.Question.Controllers
{
    [Area("Question")]
    [Route("[Area]/[Controller]/[Action]")]
    public class ProposalFormYourQuestionController : Controller
    {
        readonly IProposalFormYourQuestionService ProposalFormYourQuestionService = null;
        readonly ISiteSettingService SiteSettingService = null;

        public ProposalFormYourQuestionController
            (
                IProposalFormYourQuestionService ProposalFormYourQuestionService,
                ISiteSettingService SiteSettingService
            )
        {
            this.ProposalFormYourQuestionService = ProposalFormYourQuestionService;
            this.SiteSettingService = SiteSettingService;
        }

        [HttpPost]
        public IActionResult GetList(int? fid)
        {
            return Json(ProposalFormYourQuestionService.GetListForWeb(SiteSettingService.GetSiteSetting()?.Id, fid));
        }
    }
}
