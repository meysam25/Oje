using Oje.Infrastructure.Models;
using Oje.Section.WebMain.Models.View;
using System.Collections.Generic;

namespace Oje.Section.WebMain.Interfaces
{
    public interface IPageSliderService
    {
        ApiResult Create(PageSliderCreateUpdateVM input, int? siteSettingId, long? loginUserId);
        ApiResult Delete(long? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Update(PageSliderCreateUpdateVM input, int? siteSettingId, long? loginUserId);
        GridResultVM<PageSliderMainGridResultVM> GetList(PageSliderMainGrid searchInput, int? siteSettingId);
        List<PageWebSliderVM> GetLightList(long? pageId, int? siteSettingId);
    }
}
