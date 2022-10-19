using Oje.Infrastructure.Models;
using Oje.Section.SalesNetworkBaseData.Models.View;

namespace Oje.Section.SalesNetworkBaseData.Interfaces
{
    public interface ISalesNetworkCommissionLevelService
    {
        ApiResult Create(SalesNetworkCommissionLevelCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        GridResultVM<SalesNetworkCommissionLevelMainGridResultVM> GetList(SalesNetworkCommissionLevelMainGrid searchInput, int? siteSettingId);
        ApiResult Update(SalesNetworkCommissionLevelCreateUpdateVM input, int? siteSettingId);
    }
}
