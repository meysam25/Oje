using Oje.AccountService.Models.View;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Interfaces
{
    public interface IDashboardSectionService
    {
        ApiResult Create(DashboardSectionCreateUpdateVM input);
        ApiResult Delete(int? id);
        object GetById(int? id);
        ApiResult Update(DashboardSectionCreateUpdateVM input);
        GridResultVM<DashboardSectionGridResultVM> GetList(DashboardSectionGridFilters searchInput);
        string GetDashboardConfigByUserId(int? siteSettingId, long? userId);
    }
}
