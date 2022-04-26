using Oje.Infrastructure.Models;
using Oje.Section.WebMain.Models.View;
using System.Collections.Generic;

namespace Oje.Section.WebMain.Interfaces
{
    public interface IPageManifestService
    {
        ApiResult Create(PageManifestCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(long? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Update(PageManifestCreateUpdateVM input, int? siteSettingId);
        GridResultVM<PageManifestMainGridResultVM> GetList(PageManifestMainGrid searchInput, int? siteSettingId);
        object GetSelect2(Select2SearchVM searchInput, int? siteSettingId);
        IEnumerable<IPageWebItemVM> GetListForWeb(long? pageId, int? siteSettingId);
    }
}
