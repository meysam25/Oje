using Oje.Infrastructure.Models;
using Oje.Section.FireBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.FireBaseData.Interfaces
{
    public interface IFireInsuranceBuildingBodyService
    {
        ApiResult Create(CreateUpdateFireInsuranceBuildingBodyVM input);
        ApiResult Delete(int? id);
        CreateUpdateFireInsuranceBuildingBodyVM GetById(int? id);
        ApiResult Update(CreateUpdateFireInsuranceBuildingBodyVM input);
        GridResultVM<FireInsuranceBuildingBodyMainGridResultVM> GetList(FireInsuranceBuildingBodyMainGrid searchInput);
        object GetLightList();
    }
}
