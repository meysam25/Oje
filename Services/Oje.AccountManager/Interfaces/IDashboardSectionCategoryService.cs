using Oje.AccountService.Models.View;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Interfaces
{
    public interface IDashboardSectionCategoryService
    {
        ApiResult Create(DashboardSectionCategoryCreateUpdateVM input);
        ApiResult Delete(int? id);
        object GetById(int? id);
        ApiResult Update(DashboardSectionCategoryCreateUpdateVM input);
        GridResultVM<DashboardSectionCategoryServiceMainGridResult> GetList(DashboardSectionCategoryServiceMainGrid searchInput);
        object GetLightList();
    }
}
