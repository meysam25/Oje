using Oje.Infrastructure.Models;
using Oje.Section.Tender.Models.View;

namespace Oje.Section.Tender.Interfaces
{
    public interface ITenderFilledFormIssueService
    {
        object GetList(GlobalGridParentLong searchInput, int? siteSettingId, long? loginUserId, Infrastructure.Enums.TenderSelectStatus? selectStatus = null);
        object GetListForWeb(GlobalGridParentLong searchInput, int? siteSettingId, long? loginUserId, Infrastructure.Enums.TenderSelectStatus selectStatus);
        object Create(TenderFilledFormIssueCreateUpdateVM input, int? siteSettingId, long? loginUserId, Infrastructure.Enums.TenderSelectStatus tenderSelectStatus);
        object GetBy(long? id, int? siteSettingId, long? loginUserId, Infrastructure.Enums.TenderSelectStatus? selectStatus = null);
        object GetByForWeb(long? id, int? siteSettingId, long? loginUserId, Infrastructure.Enums.TenderSelectStatus selectStatus);
        object Update(TenderFilledFormIssueCreateUpdateVM input, int? siteSettingId, long? loginUserId, Infrastructure.Enums.TenderSelectStatus tenderSelectStatus);
    }
}
