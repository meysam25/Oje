using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;

namespace Oje.AccountService.Interfaces
{
    public interface IPropertyService
    {
        T GetBy<T>(PropertyType type, int? siteSettingId) where T : class, new();
        ApiResult CreateUpdate(object input, int? siteSettingId, PropertyType type);
        void RemoveBy(PropertyType type, int? siteSettingId);
        ApiResult Delete(PropertyType type, int? siteSettingId, string key);
    }
}
