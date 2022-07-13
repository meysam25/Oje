using Oje.Infrastructure.Models;
using Oje.Section.WebMain.Models.DB;
using Oje.Section.WebMain.Models.View;

namespace Oje.Section.WebMain.Interfaces
{
    public interface IShortLinkService
    {
        ApiResult Create(ShortLinkCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(long? id, int? siteSettingId);
        object GetById(long? id, int? siteSettingId);
        ApiResult Update(ShortLinkCreateUpdateVM input, int? siteSettingId);
        GridResultVM<ShortLinkMainGridResultVM> GetList(ShortLinkMainGrid searchInput, int? siteSettingId);
        ShortLink GetBy(int? siteSettingId, string code);
    }
}
