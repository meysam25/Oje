using Oje.Infrastructure.Models;
using Oje.Section.GlobalForms.Models.DB;
using Oje.Section.GlobalForms.Models.View;
using System.Collections.Generic;

namespace Oje.Section.GlobalForms.Interfaces
{
    public interface IGeneralFormStatusRoleService
    {
        ApiResult Create(GeneralFormStatusRoleCreateUpdateVM input);
        ApiResult Delete(int? id);
        object GetById(int? id);
        GridResultVM<GeneralFormStatusRoleMainGridResult> GetList(GeneralFormStatusRoleMainGrid searchInput);
        ApiResult Update(GeneralFormStatusRoleCreateUpdateVM input);
        List<GeneralFormStatusRole> GetByRoles(List<string> roles);
    }
}
