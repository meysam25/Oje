using Oje.EmailService.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.EmailService.Interfaces
{
    public interface IUserService
    {
        string GetUserFullName(int? siteSettingId, long? userId);
        List<RoleUsersVM> GetUserFullNameAndEmail(List<long> userIds, int? siteSettingId);
    }
}
