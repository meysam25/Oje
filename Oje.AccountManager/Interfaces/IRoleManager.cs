using Oje.AccountManager.Models.View;
using Oje.Infrastructure.Models;
using System.Collections.Generic;

namespace Oje.AccountManager.Interfaces
{
    public interface IRoleManager
    {
        ApiResult Create(CreateUpdateRoleVM input);
        ApiResult Delete(int? id);
        object GetById(int? id);
        ApiResult Update(CreateUpdateRoleVM input);
        GridResultVM<RoleGridResultVM> GetList(RoleGridFilters searchInput);
        object GetRoleLightList();
        long GetRoleValueByUserId(long UserId, int? siteSettingId);


        ApiResult CreateUser(CreateUpdateUserRoleVM input, LoginUserVM loginUserVM, int? siteSettingId);
        ApiResult DeleteUser(int? id, LoginUserVM loginUserVM, int? siteSettingId);
        object GetByIdUser(int? id, LoginUserVM loginUserVM, int? siteSettingId);
        ApiResult UpdateUser(CreateUpdateUserRoleVM input, LoginUserVM loginUserVM, int? siteSettingId);
        GridResultVM<RoleUserGridResultVM> GetListUser(RoleUserGridFilters searchInput, LoginUserVM loginUserVM, int? siteSettingId);
        long GetRoleValueByRoleId(int roleId, int? siteSettingId);
        List<int> GetRoleIdsByUserId(long? userId);
        int? GetRoleSiteSettignId(int? id);
        object GetRoleLightListForUser(LoginUserVM loginUserVM, int? siteSettingId);
        int CreateOrGetRole(string title, string name, int value);
    }
}
