using Oje.Infrastructure.Models;

namespace Oje.Section.Tender.Interfaces
{
    public interface ITenderFilledFormPFService
    {
        ApiResult AdminConfirm(long filledFormId, int jsonFormId);
        void Create(long tenderFilledFormId, int tenderProposalFormJsonConfigId);
        void DactiveConfirmation(long filledFormId, int jsonFormId);
        object GetListForWeb(GlobalGridParentLong searchInput, long? tenderFilledFormId, long? loginUserId, int? siteSettingId, Infrastructure.Enums.TenderSelectStatus? selectStatus = null);
        ApiResult UserConfirm(long filledFormId, int jsonFormId);
    }
}
