using Oje.Infrastructure.Models;
using Oje.Section.FireBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.FireBaseData.Interfaces
{
    public interface IFireInsuranceCoverageTitleManager
    {
        ApiResult Create(CreateUpdateFireInsuranceCoverageTitleVM input);
        ApiResult Delete(int? id);
        CreateUpdateFireInsuranceCoverageTitleVM GetById(int? id);
        ApiResult Update(CreateUpdateFireInsuranceCoverageTitleVM input);
        GridResultVM<FireInsuranceCoverageTitleMainGridResultVM> GetList(FireInsuranceCoverageTitleMainGrid searchInput);
        object GetSelect2List(Select2SearchVM searchInput);
    }
}
