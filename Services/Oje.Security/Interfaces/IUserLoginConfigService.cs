using Oje.Infrastructure.Models;
using Oje.Security.Models.DB;
using Oje.Security.Models.View;

namespace Oje.Security.Interfaces
{
    public interface IUserLoginConfigService
    {
        ApiResult CreateUpdate(UserLoginConfigCreateUpdateVM input, int? siteSettingId);
        object GetBy(int? siteSettingId);
        UserLoginConfig GetByCache(int? siteSettingId);
    }
}
