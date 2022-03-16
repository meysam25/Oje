using Oje.AccountService.Models;
using Oje.AccountService.Models.DB;
using Oje.AccountService.Models.View;
using Oje.Infrastructure.Models;
using System.Collections.Generic;

namespace Oje.AccountService.Interfaces
{
    public interface ISectionService
    {
        void UpdateModuals();
        List<Section> GetSideMenu(long? userId);
        List<SiteMenueVM> GetSideMenuWidthCategory(long? userId);
        object GetSideMenuAjax(long? userId);
        object GetListForTreeView(int? id);
        ApiResult UpdateAccess(CreateUpdateRoleAccessVM input);
        List<AccessTreeViewUser> GetListForTreeViewForUser(int? id, LoginUserVM loginUserVM, int? siteSettingId);
        object UpdateAccessForUser(CreateUpdateRoleAccessVM input, LoginUserVM loginUserVM, int? siteSettingId);
        object GetSelect2List(Select2SearchVM searchInput);
    }
}
