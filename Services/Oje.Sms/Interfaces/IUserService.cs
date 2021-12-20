using Oje.Sms.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Sms.Interfaces
{
    public interface IUserService
    {
        string GetUserFullName(int? siteSettingId, long? userId);
        List<RoleUsersVM> GetUserFullNameAndMobile(List<long> userIds, int? siteSettingId);
    }
}
