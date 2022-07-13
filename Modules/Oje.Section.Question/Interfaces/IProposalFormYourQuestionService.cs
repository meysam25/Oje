using Oje.Infrastructure.Models;
using Oje.Section.Question.Models.View;

namespace Oje.Section.Question.Interfaces
{
    public interface IProposalFormYourQuestionService
    {
        ApiResult Create(ProposalFormYourQuestionCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Update(ProposalFormYourQuestionCreateUpdateVM input, int? siteSettingId);
        GridResultVM<ProposalFormYourQuestionMainGridResultVM> GetList(ProposalFormYourQuestionMainGrid searchInput, int? siteSettingId);
        object GetListForWeb(int? siteSettingId, int? formid);
    }
}
