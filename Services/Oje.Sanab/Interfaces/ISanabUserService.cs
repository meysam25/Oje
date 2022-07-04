using Oje.Sanab.Models.DB;

namespace Oje.Sanab.Interfaces
{
    public interface ISanabUserService
    {
        SanabUser GetActive(int? siteSettingId);
    }
}
