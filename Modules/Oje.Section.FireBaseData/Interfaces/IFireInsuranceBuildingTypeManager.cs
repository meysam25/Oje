using Oje.Infrastructure.Models;
using Oje.Section.FireBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.FireBaseData.Interfaces
{
    public interface IFireInsuranceBuildingTypeManager
    {
        ApiResult Create(CreateUpdateFireInsuranceBuildingTypeVM input);
        ApiResult Delete(int? id);
        CreateUpdateFireInsuranceBuildingTypeVM GetById(int? id);
        ApiResult Update(CreateUpdateFireInsuranceBuildingTypeVM input);
        GridResultVM<FireInsuranceBuildingTypeMainGridResultVM> GetList(FireInsuranceBuildingTypeMainGrid searchInput);
        object GetLightList();
    }
}
