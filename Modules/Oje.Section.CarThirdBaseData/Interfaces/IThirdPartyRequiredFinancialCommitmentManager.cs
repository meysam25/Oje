using Oje.Infrastructure.Models;
using Oje.Section.CarThirdBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarThirdBaseData.Interfaces
{
    public interface IThirdPartyRequiredFinancialCommitmentManager
    {
        ApiResult Create(CreateUpdateThirdPartyRequiredFinancialCommitmentVM input);
        ApiResult Delete(int? id);
        CreateUpdateThirdPartyRequiredFinancialCommitmentVM GetById(int? id);
        ApiResult Update(CreateUpdateThirdPartyRequiredFinancialCommitmentVM input);
        GridResultVM<ThirdPartyRequiredFinancialCommitmentMainGridResultVM> GetList(ThirdPartyRequiredFinancialCommitmentMainGrid searchInput);
        object GetSelect2List(Select2SearchVM searchInput);
    }
}
