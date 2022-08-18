using Oje.Infrastructure.Models;

namespace Oje.Section.RegisterForm.Interfaces
{
    public interface IRoleService
    {
        object GetList(int? siteSettingId, Select2SearchVM searchInput);
        object GetLightList(int? siteSettingId);
    }
}
