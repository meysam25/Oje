using Oje.Infrastructure.Models;
using Oje.Section.CarThirdBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarThirdBaseData.Interfaces
{
    public interface IThirdPartyDriverNoDamageDiscountHistoryManager
    {
        ApiResult Create(CreateUpdateThirdPartyDriverNoDamageDiscountHistoryVM input);
        ApiResult Delete(int? id);
        CreateUpdateThirdPartyDriverNoDamageDiscountHistoryVM GetById(int? id);
        ApiResult Update(CreateUpdateThirdPartyDriverNoDamageDiscountHistoryVM input);
        GridResultVM<ThirdPartyDriverNoDamageDiscountHistoryMainGridResultVM> GetList(ThirdPartyDriverNoDamageDiscountHistoryMainGrid searchInput);
    }
}
