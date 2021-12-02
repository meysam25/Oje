using Oje.Infrastructure.Models;
using Oje.Section.FireBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.FireBaseData.Interfaces
{
    public interface IFireInsuranceCoverageActivityDangerLevelManager
    {
        ApiResult Create(CreateUpdateFireInsuranceCoverageActivityDangerLevelVM input);
        ApiResult Delete(int? id);
        CreateUpdateFireInsuranceCoverageActivityDangerLevelVM GetById(int? id);
        ApiResult Update(CreateUpdateFireInsuranceCoverageActivityDangerLevelVM input);
        GridResultVM<FireInsuranceCoverageActivityDangerLevelMainGridResultVM> GetList(FireInsuranceCoverageActivityDangerLevelMainGrid searchInput);
    }
}
