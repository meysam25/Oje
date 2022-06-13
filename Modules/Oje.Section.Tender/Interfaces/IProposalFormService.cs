
using Oje.Infrastructure.Models;

namespace Oje.Section.Tender.Interfaces
{
    public interface IProposalFormService
    {
        bool Exist(int? id, int? siteSettingId);
        object GetSelect2List(Select2SearchVM searchInput, int? siteSettingId);
        object GetSelect2List(Select2SearchVM searchInput, int? siteSettingId, int? insuranceCatId);
    }
}
