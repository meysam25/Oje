using Oje.Infrastructure.Models;
using Oje.Section.CarThirdBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarThirdBaseData.Interfaces
{
    public interface IThirdPartyDriverFinancialCommitmentManager
    {
        ApiResult Create(CreateUpdateThirdPartyDriverFinancialCommitmentVM input);
        ApiResult Delete(int? id);
        CreateUpdateThirdPartyDriverFinancialCommitmentVM GetById(int? id);
        ApiResult Update(CreateUpdateThirdPartyDriverFinancialCommitmentVM input);
        GridResultVM<ThirdPartyDriverFinancialCommitmentMainGridResultVM> GetList(ThirdPartyDriverFinancialCommitmentMainGrid searchInput);
    }
}
