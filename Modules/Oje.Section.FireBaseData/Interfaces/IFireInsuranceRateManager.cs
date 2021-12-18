using Oje.Infrastructure.Models;
using Oje.Section.FireBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.FireBaseData.Interfaces
{
    public interface IFireInsuranceRateService
    {
        ApiResult Create(CreateUpdateFireInsuranceRateVM input);
        ApiResult Delete(int? id);
        CreateUpdateFireInsuranceRateVM GetById(int? id);
        ApiResult Update(CreateUpdateFireInsuranceRateVM input);
        GridResultVM<FireInsuranceRateMainGridResultVM> GetList(FireInsuranceRateMainGrid searchInput);

    }
}
