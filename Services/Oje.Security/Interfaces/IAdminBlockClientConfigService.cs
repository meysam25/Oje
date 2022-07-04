using Oje.Infrastructure.Models;
using Oje.Security.Models.DB;
using Oje.Security.Models.View;

namespace Oje.Security.Interfaces
{
    public interface IAdminBlockClientConfigService
    {
        ApiResult Create(AdminBlockClientConfigCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Update(AdminBlockClientConfigCreateUpdateVM input, int? siteSettingId);
        GridResultVM<AdminBlockClientConfigMainGridResultVM> GetList(AdminBlockClientConfigMainGrid searchInput, int? siteSettingId);
        AdminBlockClientConfig GetByFromCache(long actionId, int siteSettingId);
    }
}
