using Oje.Infrastructure.Models;
using Oje.Section.FireBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.FireBaseData.Interfaces
{
    public interface IFireInsuranceCoverageService
    {
        ApiResult Create(CreateUpdateFireInsuranceCoverageVM input);
        ApiResult Delete(int? id);
        CreateUpdateFireInsuranceCoverageVM GetById(int? id);
        ApiResult Update(CreateUpdateFireInsuranceCoverageVM input);
        GridResultVM<FireInsuranceCoverageMainGridResultVM> GetList(FireInsuranceCoverageMainGrid searchInput);
    }
}
