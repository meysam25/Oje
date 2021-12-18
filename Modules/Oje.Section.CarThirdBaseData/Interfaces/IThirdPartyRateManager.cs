using Oje.Infrastructure.Models;
using Oje.Section.CarThirdBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarThirdBaseData.Interfaces
{
    public interface IThirdPartyRateService
    {
        ApiResult Create(CreateUpdateThirdPartyRateVM input);
        ApiResult Delete(int? id);
        CreateUpdateThirdPartyRateVM GetById(int? id);
        ApiResult Update(CreateUpdateThirdPartyRateVM input);
        GridResultVM<ThirdPartyRateMainGridResultVM> GetList(ThirdPartyRateMainGrid searchInput);
    }
}
