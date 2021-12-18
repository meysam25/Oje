using Oje.Sms.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Sms.Interfaces
{
    public interface IRoleService
    {
        List<RoleUsersVM> GetUsersBy(List<int> roleIds, int? siteSettingId, int count);
    }
}
