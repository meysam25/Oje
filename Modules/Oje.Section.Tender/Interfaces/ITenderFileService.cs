using Oje.Infrastructure.Models;
using Oje.Section.Tender.Models.View;

namespace Oje.Section.Tender.Interfaces
{
    public interface ITenderFileService
    {
        ApiResult Create(TenderFileCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        GridResultVM<TenderFileMainGridResultVM> GetList(TenderFileMainGrid searchInput, int? siteSettingId);
        object GetListForWeb(int? siteSettingId);
        ApiResult Update(TenderFileCreateUpdateVM input, int? siteSettingId);
    }
}
