using Oje.Infrastructure.Models;
using Oje.Section.WebMain.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.WebMain.Interfaces
{
    public interface IPageLeftRightDesignService
    {
        ApiResult Create(PageLeftRightDesignCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(long? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Update(PageLeftRightDesignCreateUpdateVM input, int? siteSettingId);
        GridResultVM<PageLeftRightDesignMainGridResultVM> GetList(PageLeftRightDesignMainGrid searchInput, int? siteSettingId);
        object GetSelect2(Select2SearchVM searchInput, int? siteSettingId);
        List<IPageWebItemVM> GetListForWeb(long? pageId, int? siteSettingId);
    }
}
