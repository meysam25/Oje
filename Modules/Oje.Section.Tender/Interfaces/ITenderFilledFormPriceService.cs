using Oje.Infrastructure.Models;
using Oje.Section.Tender.Models.View;

namespace Oje.Section.Tender.Interfaces
{
    public interface ITenderFilledFormPriceService
    {
        GridResultVM<TenderFilledFormPriceMainGridResultVM> GetList(TenderFilledFormPriceMainGrid searchInput, int? siteSettingId, long? loginUserId, Infrastructure.Enums.TenderSelectStatus? selectStatus = null);
        GridResultVM<TenderFilledFormPriceMainGridResultVM> GetListForWeb(TenderFilledFormPriceMainGrid searchInput, int? siteSettingId, long? loginUserId, Infrastructure.Enums.TenderSelectStatus selectStatus);
        ApiResult Create(TenderFilledFormPriceCreateUpdateVM input, int? siteSettingId, long? loginUserId, Infrastructure.Enums.TenderSelectStatus tenderSelectStatus);
        object GetById(long? id, int? siteSettingId, long? userId, Infrastructure.Enums.TenderSelectStatus? selectStatus = null);
        object Update(TenderFilledFormPriceCreateUpdateVM input, int? siteSettingId, long? loginUserId, Infrastructure.Enums.TenderSelectStatus tenderSelectStatus);
        object Delete(long? id, int? siteSettingId, long? loginUserId, Infrastructure.Enums.TenderSelectStatus tenderSelectStatus);
        object GetUsersForWeb(long? tenderFilledFormId, int? siteSettingId, long? loginUserId, Infrastructure.Enums.TenderSelectStatus selectStatus);
        object SelectPrice(MyTenderFilledFormPriceSelectUserUpdateVM input, int? siteSettingId, long? loginUserId, Infrastructure.Enums.TenderSelectStatus selectStatus);
        object Publish(long? id, int? siteSettingId, long? loginUserId, Infrastructure.Enums.TenderSelectStatus tenderSelectStatus);
        int? GetCompanyIdBy(long? tenderFilledFormId, int? tenderProposalFormJsonConfigId, int? siteSettingId, long? loginUserId);
    }
}
