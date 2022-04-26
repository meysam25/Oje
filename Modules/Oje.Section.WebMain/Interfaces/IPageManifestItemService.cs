
using Oje.Infrastructure.Models;
using Oje.Section.WebMain.Models.View;

namespace Oje.Section.WebMain.Interfaces
{
    public interface IPageManifestItemService
    {
        ApiResult Create(PageManifestItemCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(long? id, int? siteSettingId);
        PageManifestItemCreateUpdateVM GetById(long? id, int? siteSettingId);
        ApiResult Update(PageManifestItemCreateUpdateVM input, int? siteSettingId);
        GridResultVM<PageManifestItemMainGridResultVM> GetList(PageManifestItemMainGrid searchInput, int? siteSettingId);
    }
}
