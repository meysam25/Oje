using Oje.Infrastructure.Models;
using Oje.Section.FireBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.FireBaseData.Interfaces
{
    public interface IFireInsuranceBuildingUnitValueService
    {
        ApiResult Create(CreateUpdateFireInsuranceBuildingUnitValueVM input);
        ApiResult Delete(int? id);
        CreateUpdateFireInsuranceBuildingUnitValueVM GetById(int? id);
        ApiResult Update(CreateUpdateFireInsuranceBuildingUnitValueVM input);
        GridResultVM<FireInsuranceBuildingUnitValueMainGridResultVM> GetList(FireInsuranceBuildingUnitValueMainGrid searchInput);
        object GetLightList();
    }
}
