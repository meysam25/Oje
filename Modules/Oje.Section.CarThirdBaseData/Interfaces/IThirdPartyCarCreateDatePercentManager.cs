using Oje.Infrastructure.Models;
using Oje.Section.CarThirdBaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarThirdBaseData.Interfaces
{
    public interface IThirdPartyCarCreateDatePercentService
    {
        ApiResult Create(CreateUpdateThirdPartyCarCreateDatePercentVM input);
        ApiResult Delete(int? id);
        CreateUpdateThirdPartyCarCreateDatePercentVM GetById(int? id);
        ApiResult Update(CreateUpdateThirdPartyCarCreateDatePercentVM input);
        GridResultVM<ThirdPartyCarCreateDatePercentMainGridResultVM> GetList(ThirdPartyCarCreateDatePercentMainGrid searchInput);
    }
}
