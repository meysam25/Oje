using Oje.Infrastructure.Models;
using Oje.Section.Tender.Models.View;

namespace Oje.Section.Tender.Interfaces
{
    public interface ITenderFilledFormIssueService
    {
        object GetList(GlobalGridParentLong searchInput, int? siteSettingId, long? loginUserId);
        object GetListForWeb(GlobalGridParentLong searchInput, int? siteSettingId, long? loginUserId);
        object Create(TenderFilledFormIssueCreateUpdateVM input, int? siteSettingId, long? loginUserId);
        object GetBy(long? id, int? siteSettingId, long? loginUserId);
        object GetByForWeb(long? id, int? siteSettingId, long? loginUserId);
        object Update(TenderFilledFormIssueCreateUpdateVM input, int? siteSettingId, long? loginUserId);
    }
}
