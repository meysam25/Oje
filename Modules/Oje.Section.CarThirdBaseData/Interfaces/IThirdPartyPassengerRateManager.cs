using Oje.Infrastructure.Models;
using Oje.Section.CarThirdBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarThirdBaseData.Interfaces
{
    public interface IThirdPartyPassengerRateManager
    {
        ApiResult Create(CreateUpdateThirdPartyPassengerRateVM input);
        ApiResult Delete(int? id);
        CreateUpdateThirdPartyPassengerRateVM GetById(int? id);
        ApiResult Update(CreateUpdateThirdPartyPassengerRateVM input);
        GridResultVM<ThirdPartyPassengerRateMainGridResultVM> GetList(ThirdPartyPassengerRateMainGrid searchInput);
    }
}
