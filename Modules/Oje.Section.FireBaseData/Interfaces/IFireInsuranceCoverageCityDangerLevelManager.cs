using Oje.Infrastructure.Models;
using Oje.Section.FireBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.FireBaseData.Interfaces
{
    public interface IFireInsuranceCoverageCityDangerLevelManager
    {
        ApiResult Create(CreateUpdateFireInsuranceCoverageCityDangerLevelVM input);
        ApiResult Delete(int? id);
        CreateUpdateFireInsuranceCoverageCityDangerLevelVM GetById(int? id);
        ApiResult Update(CreateUpdateFireInsuranceCoverageCityDangerLevelVM input);
        GridResultVM<FireInsuranceCoverageCityDangerLevelMainGridResultVM> GetList(FireInsuranceCoverageCityDangerLevelMainGrid searchInput);
    }
}
