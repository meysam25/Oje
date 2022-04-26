using Oje.Infrastructure.Models;
using Oje.Section.WebMain.Models.View;

namespace Oje.Section.WebMain.Interfaces
{
    public interface IPageService
    {
        ApiResult Create(PageCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(long? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Update(PageCreateUpdateVM input, int? siteSettingId);
        GridResultVM<PageMainGridResultVM> GetList(PageMainGrid searchInput, int? siteSettingId);
        object GetSelect2(Select2SearchVM searchInput, int? siteSettingId);
        PageWebVM GetBy(long? id, string pTitle, int? siteSettingId);
        string GenerateUrlForPage(string title, long? pageId);
        bool Exist(long? id, int? siteSettingId);
    }
}
