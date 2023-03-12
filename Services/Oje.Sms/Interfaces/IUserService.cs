using Oje.Sms.Models.DB;
using Oje.Sms.Models.View;

namespace Oje.Sms.Interfaces
{
    public interface IUserService
    {
        string GetUserFullName(int? siteSettingId, long? userId);
        List<RoleUsersVM> GetUserFullNameAndMobile(List<long> userIds, int? siteSettingId);
        User GetBy(int? siteSettingId, string username);
    }
}
