using Oje.Infrastructure.Models;
using Oje.Section.FireBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.FireBaseData.Interfaces
{
    public interface IFireInsuranceBuildingAgeManager
    {
        ApiResult Create(CreateUpdateFireInsuranceBuildingAgeVM input);
        ApiResult Delete(int? id);
        CreateUpdateFireInsuranceBuildingAgeVM GetById(int? id);
        ApiResult Update(CreateUpdateFireInsuranceBuildingAgeVM input);
        GridResultVM<FireInsuranceBuildingAgeMainGridResultVM> GetList(FireInsuranceBuildingAgeMainGrid searchInput);
    }
}
