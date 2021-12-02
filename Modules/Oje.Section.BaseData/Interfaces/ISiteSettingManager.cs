
using Oje.Infrastructure.Models;
using Oje.Section.BaseData.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.BaseData.Interfaces
{
    public interface ISiteSettingManager
    {
        ApiResult Create(CreateUpdateSiteSettingVM input, long? userId);
        ApiResult Delete(int? id);
        object GetById(int? id);
        ApiResult Update(CreateUpdateSiteSettingVM input, long? userId);
        GridResultVM<SiteSettingGridList> GetList(SiteSettingMainGrid searchInput);
        object GetLightList();
    }
}
