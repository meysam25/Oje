using Oje.Infrastructure.Models;
using Oje.Section.CarThirdBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarThirdBaseData.Interfaces
{
    public interface IThirdPartyFinancialCommitmentManager
    {
        ApiResult Create(CreateUpdateThirdPartyFinancialCommitmentVM input);
        ApiResult Delete(int? id);
        CreateUpdateThirdPartyFinancialCommitmentVM GetById(int? id);
        ApiResult Update(CreateUpdateThirdPartyFinancialCommitmentVM input);
        GridResultVM<ThirdPartyFinancialCommitmentMainGridResultVM> GetList(ThirdPartyFinancialCommitmentMainGrid searchInput);
    }
}
