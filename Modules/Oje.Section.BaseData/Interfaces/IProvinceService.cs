using Oje.Infrastructure.Models;
using Oje.Section.BaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.BaseData.Interfaces
{
    public interface IProvinceService
    {
        ApiResult Create(CreateUpdateProvinceVM input);
        ApiResult Delete(int? id);
        CreateUpdateProvinceVM GetById(int? id);
        ApiResult Update(CreateUpdateProvinceVM input);
        GridResultVM<ProvinceMainGridResultVM> GetList(ProvinceMainGrid searchInput);
        object GetLightList();
    }
}
