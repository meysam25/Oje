using Oje.Infrastructure.Models;

namespace Oje.Section.SalesNetworkBaseData.Interfaces
{
    public interface IProposalFormService
    {
        object GetSelect2List(Select2SearchVM searchInput, int? siteSettingId);
        bool Exist(int id, int? siteSettingId);
    }
}
