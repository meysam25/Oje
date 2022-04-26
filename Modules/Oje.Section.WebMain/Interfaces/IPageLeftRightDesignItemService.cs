using Oje.Infrastructure.Models;
using Oje.Section.WebMain.Models.View;

namespace Oje.Section.WebMain.Interfaces
{
    public interface IPageLeftRightDesignItemService
    {
        ApiResult Create(PageLeftRightDesignItemCreateUpdateVM input, int? siteSettingId);
        ApiResult Update(PageLeftRightDesignItemCreateUpdateVM input, int? siteSettingId);
        PageLeftRightDesignItemCreateUpdateVM GetById(long? id, int? siteSettingId);
        ApiResult Delete(long? id, int? siteSettingId);
        GridResultVM<PageLeftRightDesignItemMainGridResultVM> GetList(PageLeftRightDesignItemMainGrid searchInput, int? siteSettingId);
    }
}
