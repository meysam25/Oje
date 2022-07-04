using Oje.Infrastructure.Models;
using Oje.Security.Models.View;

namespace Oje.Security.Interfaces
{
    public interface IUserAdminLogService
    {
        void Create(long loginUserId, string requestId, long actionId, bool isSuccess, bool isStart, IpSections ip, int siteSettingId);
        GridResultVM<UserAdminLogMainGridResultVM> GetList(UserAdminLogMainGrid searchInput, int? siteSettingId);
    }
}
