using Oje.Infrastructure.Models;
using Oje.Security.Models.View;

namespace Oje.Security.Interfaces
{
    public interface IBlockLoginUserService
    {
        ApiResult Create(BlockLoginUserCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Update(BlockLoginUserCreateUpdateVM input, int? siteSettingId);
        GridResultVM<BlockLoginUserMainGridResultVM> GetList(BlockLoginUserMainGrid searchInput, int? siteSettingId);
        bool IsValidDay(DateTime targetDate, int? siteSettingId);
    }
}
