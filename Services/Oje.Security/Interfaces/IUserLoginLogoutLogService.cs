using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.Security.Models.View;

namespace Oje.Security.Interfaces
{
    public interface IUserLoginLogoutLogService
    {
        void Create(long userId, UserLoginLogoutLogType type, int? siteSettingId, bool isSuccess, string message);
        GridResultVM<UserLoginLogoutLogMainGridResultVM> GetList(UserLoginLogoutLogMainGrid searchInput, int? siteSettingId);
    }
}
