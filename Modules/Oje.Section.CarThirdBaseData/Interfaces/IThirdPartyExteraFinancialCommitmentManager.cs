using Oje.Infrastructure.Models;
using Oje.Section.CarThirdBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarThirdBaseData.Interfaces
{
    public interface IThirdPartyExteraFinancialCommitmentManager
    {
        ApiResult Create(CreateUpdateThirdPartyExteraFinancialCommitmentVM input);
        ApiResult Delete(int? id);
        CreateUpdateThirdPartyExteraFinancialCommitmentVM GetById(int? id);
        ApiResult Update(CreateUpdateThirdPartyExteraFinancialCommitmentVM input);
        GridResultVM<ThirdPartyExteraFinancialCommitmentMainGridResultVM> GetList(ThirdPartyExteraFinancialCommitmentMainGrid searchInput);
        ApiResult CreateFromExcel(GlobalExcelFile input);
    }
}
