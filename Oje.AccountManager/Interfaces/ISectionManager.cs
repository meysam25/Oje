using Oje.AccountManager.Models;
using Oje.AccountManager.Models.DB;
using Oje.AccountManager.Models.View;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountManager.Interfaces
{
    public interface ISectionManager
    {
        void UpdateModuals();
        List<Section> GetSideMenu(long? userId);
        object GetSideMenuAjax(long? userId);
        object GetListForTreeView(int? id);
        ApiResult UpdateAccess(CreateUpdateRoleAccessVM input);
        List<AccessTreeViewUser> GetListForTreeViewForUser(int? id, LoginUserVM loginUserVM, int? siteSettingId);
        object UpdateAccessForUser(CreateUpdateRoleAccessVM input, LoginUserVM loginUserVM, int? siteSettingId);
    }
}
