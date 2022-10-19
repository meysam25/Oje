using Oje.Infrastructure.Models;
using Oje.Section.SalesNetworkBaseData.Models.View;

namespace Oje.Section.SalesNetworkBaseData.Interfaces
{
    public interface ISalesNetworkService
    {
        ApiResult Create(CreateUpdateSalesNetworkVM input);
        ApiResult Delete(int? id);
        object GetById(int? id);
        ApiResult Update(CreateUpdateSalesNetworkVM input);
        GridResultVM<SalesNetworkMainGridResulgVM> GetList(SalesNetworkMainGrid searchInput);
        object GetLightListMultiLevel(int? siteSettingId);
        GridResultVM<SalesNetworkReportMainGridResultVM> GetReportList(SalesNetworkReportMainGrid searchInput, int? siteSettingId);
        object GetUserListBySaleNetworkId(int? siteSettingId, int? id, Select2SearchVM searchInput);
        object GetReportChart(int? siteSettingId, SalesNetworkReportMainGrid searchInput);
    }
}
