using Oje.Infrastructure.Models;
using Oje.Section.Tender.Models.View;

namespace Oje.Section.Tender.Interfaces
{
    public interface ITenderFilledFormPriceService
    {
        GridResultVM<TenderFilledFormPriceMainGridResultVM> GetList(TenderFilledFormPriceMainGrid searchInput, int? siteSettingId, long? loginUserId);
        GridResultVM<TenderFilledFormPriceMainGridResultVM> GetListForWeb(TenderFilledFormPriceMainGrid searchInput, int? siteSettingId, long? loginUserId);
        ApiResult Create(TenderFilledFormPriceCreateUpdateVM input, int? siteSettingId, long? loginUserId);
        object GetById(long? id, int? siteSettingId, long? userId);
        object Update(TenderFilledFormPriceCreateUpdateVM input, int? siteSettingId, long? loginUserId);
        object Delete(long? id, int? siteSettingId, long? loginUserId);
        object GetUsersForWeb(long? tenderFilledFormId, int? siteSettingId, long? loginUserId);
        object SelectPrice(MyTenderFilledFormPriceSelectUserUpdateVM input, int? siteSettingId, long? loginUserId);
        object Publish(long? id, int? siteSettingId, long? loginUserId);
        int? GetCompanyIdBy(long? tenderFilledFormId, int? tenderProposalFormJsonConfigId, int? siteSettingId, long? loginUserId);
    }
}
