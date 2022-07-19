using Oje.Infrastructure.Models;
using Oje.Section.WebMain.Models.View;

namespace Oje.Section.WebMain.Interfaces
{
    public interface ILoginBackgroundImageService
    {
        ApiResult Create(LoginBackgroundImageCreateUpdateVM input, int? siteSettingId, long? loginUserId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Update(LoginBackgroundImageCreateUpdateVM input, int? siteSettingId, long? loginUserId);
        GridResultVM<LoginBackgroundImageMainGridResultVM> GetList(LoginBackgroundImageMainGrid searchInput, int? siteSettingId);
    }
}
