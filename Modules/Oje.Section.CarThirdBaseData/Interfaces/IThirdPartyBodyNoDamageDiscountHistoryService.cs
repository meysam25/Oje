using Oje.Infrastructure.Models;
using Oje.Section.CarThirdBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarThirdBaseData.Interfaces
{
    public interface IThirdPartyBodyNoDamageDiscountHistoryService
    {
        ApiResult Create(CreateUpdateThirdPartyBodyNoDamageDiscountHistoryVM input);
        ApiResult Delete(int? id);
        CreateUpdateThirdPartyBodyNoDamageDiscountHistoryVM GetById(int? id);
        ApiResult Update(CreateUpdateThirdPartyBodyNoDamageDiscountHistoryVM input);
        GridResultVM<ThirdPartyBodyNoDamageDiscountHistoryMainGridResultVM> GetList(ThirdPartyBodyNoDamageDiscountHistoryMainGrid searchInput);
    }
}
