using Oje.Infrastructure.Models;
using Oje.Security.Models.View;

namespace Oje.Security.Interfaces
{
    public interface IUserAdminLogConfigService
    {
        ApiResult Create(UserAdminLogConfigCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(long? id, int? siteSettingId);
        object GetById(long? id, int? siteSettingId);
        ApiResult Update(UserAdminLogConfigCreateUpdateVM input, int? siteSettingId);
        GridResultVM<UserAdminLogConfigMainGridResultVM> GetList(UserAdminLogConfigMainGrid searchInput, int? siteSettingId);
        bool IsNeededCache(long actionId, int siteSettingId);
    }
}
