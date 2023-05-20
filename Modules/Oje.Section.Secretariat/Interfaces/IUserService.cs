using Oje.Infrastructure.Models;

namespace Oje.Section.Secretariat.Interfaces
{
    public interface IUserService
    {
        bool Exist(long? id, int? siteSettingId);
        long? GetOrCreateByUsername(string mobile, int siteSettingId, string fullname, long userId);
        object GetSelect2List(Select2SearchVM searchInput, int? siteSettingId);
    }
}
