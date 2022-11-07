using Microsoft.AspNetCore.Http;

namespace Oje.Section.GlobalForms.Interfaces
{
    public interface IInternalUserService
    {
        void UpdateUserInfoIfNeeded(IFormCollection form, long userId, int? siteSettingId);
    }
}
