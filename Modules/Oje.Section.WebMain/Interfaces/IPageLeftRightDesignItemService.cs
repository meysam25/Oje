using Oje.Infrastructure.Models;
using Oje.Section.WebMain.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Interfaces
{
    public interface IPageLeftRightDesignItemService
    {
        ApiResult Create(PageLeftRightDesignItemCreateUpdateVM input, int? siteSettingId);
        ApiResult Update(PageLeftRightDesignItemCreateUpdateVM input, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Delete(long? id, int? siteSettingId);
        GridResultVM<PageLeftRightDesignItemMainGridResultVM> GetList(PageLeftRightDesignItemMainGrid searchInput, int? siteSettingId);
    }
}
