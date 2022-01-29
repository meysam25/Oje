using Oje.Infrastructure.Models;
using Oje.Security.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Security.Interfaces
{
    public interface IFileAccessRoleService
    {
        ApiResult Create(CreateUpdateFileAccessRoleVM input);
        ApiResult Delete(int? id);
        object GetById(int? id);
        ApiResult Update(CreateUpdateFileAccessRoleVM input);
        GridResultVM<FileAccessRoleMainGridResultVM> GetList(FileAccessRoleMainGrid searchInput);
    }
}
