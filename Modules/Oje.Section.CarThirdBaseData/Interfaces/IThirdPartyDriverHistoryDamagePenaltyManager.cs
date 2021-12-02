using Oje.Infrastructure.Models;
using Oje.Section.CarThirdBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarThirdBaseData.Interfaces
{
    public interface IThirdPartyDriverHistoryDamagePenaltyManager
    {
        ApiResult Create(CreateUpdateThirdPartyDriverHistoryDamagePenaltyVM input);
        ApiResult Delete(int? id);
        CreateUpdateThirdPartyDriverHistoryDamagePenaltyVM GetById(int? id);
        ApiResult Update(CreateUpdateThirdPartyDriverHistoryDamagePenaltyVM input);
        GridResultVM<ThirdPartyDriverHistoryDamagePenaltyMainGridResultVM> GetList(ThirdPartyDriverHistoryDamagePenaltyMainGrid searchInput);
    }
}
