using Oje.Infrastructure.Models;

namespace Oje.Section.Tender.Interfaces
{
    public interface ITenderFilledFormPFService
    {
        void Create(long tenderFilledFormId, int tenderProposalFormJsonConfigId);
        object GetListForWeb(GlobalGridParentLong searchInput, long? tenderFilledFormId, long? loginUserId, int? siteSettingId);
    }
}
