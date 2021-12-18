using Oje.Infrastructure.Models;
using Oje.Section.BaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.BaseData.Interfaces
{
    public interface IDutyService
    {
        ApiResult Create(CreateUpdateDutyVM input);
        ApiResult Delete(int? id);
        CreateUpdateDutyVM GetById(int? id);
        ApiResult Update(CreateUpdateDutyVM input);
        GridResultVM<DutyMainGridResultVM> GetList(DutyMainGrid searchInput);
    }
}
