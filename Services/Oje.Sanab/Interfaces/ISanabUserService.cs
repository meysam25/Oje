using Oje.Infrastructure.Models;
using Oje.Sanab.Models.DB;
using Oje.Sanab.Models.View;

namespace Oje.Sanab.Interfaces
{
    public interface ISanabUserService
    {
        SanabUser GetActive(int? siteSettingId);
        object GetById(int? siteSettingId);
        ApiResult CreateUpdate(SanabUserCreateUpdateVM input, int? siteSettingId);
    }
}
