using Oje.Infrastructure.Models;
using Oje.Section.CarThirdBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarThirdBaseData.Interfaces
{
    public interface IThirdPartyFinancialAndBodyHistoryDamagePenaltyService
    {
        ApiResult Create(CreateUpdateThirdPartyFinancialAndBodyHistoryDamagePenaltyVM input);
        ApiResult Delete(int? id);
        CreateUpdateThirdPartyFinancialAndBodyHistoryDamagePenaltyVM GetById(int? id);
        ApiResult Update(CreateUpdateThirdPartyFinancialAndBodyHistoryDamagePenaltyVM input);
        GridResultVM<ThirdPartyFinancialAndBodyHistoryDamagePenaltyMainGridResultVM> GetList(ThirdPartyFinancialAndBodyHistoryDamagePenaltyMainGrid searchInput);
    }
}
