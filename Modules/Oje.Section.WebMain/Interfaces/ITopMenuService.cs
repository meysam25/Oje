using Oje.Infrastructure.Models;
using Oje.Section.WebMain.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Interfaces
{
    public interface ITopMenuService
    {
        ApiResult Create(TopMenuCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(long? id, int? siteSettingId);
        object GetById(long? id, int? siteSettingId);
        ApiResult Update(TopMenuCreateUpdateVM input, int? siteSettingId);
        GridResultVM<TopMenuMainGridResultVM> GetList(TopMenuMainGrid searchInput, int? siteSettingId);
        object GetListForWeb(int? siteSettingId);
    }
}
