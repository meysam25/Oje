using Oje.Infrastructure.Models;

namespace Oje.Section.Question.Interfaces
{
    public interface IProposalFormService
    {
        object GetSelect2List(Select2SearchVM searchInput, int? siteSettingId);
        bool Exist(int? siteSettingId, int? id);
    }
}
