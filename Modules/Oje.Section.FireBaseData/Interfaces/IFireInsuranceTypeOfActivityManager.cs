using Oje.Infrastructure.Models;
using Oje.Section.FireBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.FireBaseData.Interfaces
{
    public interface IFireInsuranceTypeOfActivityManager
    {
        ApiResult Create(CreateUpdateFireInsuranceTypeOfActivityVM input);
        ApiResult Delete(int? id);
        CreateUpdateFireInsuranceTypeOfActivityVM GetById(int? id);
        ApiResult Update(CreateUpdateFireInsuranceTypeOfActivityVM input);
        GridResultVM<FireInsuranceTypeOfActivityMainGridResultVM> GetList(FireInsuranceTypeOfActivityMainGrid searchInput);
    }
}
