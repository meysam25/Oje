using Oje.AccountService.Models.DB;
using Oje.AccountService.Models.View;
using Oje.Infrastructure.Models;

namespace Oje.AccountService.Interfaces
{
    public interface IUserNotificationService
    {
        void Create(UserNotification userNotification, int? siteSettingId);
        void SaveChange();
        int? GetUserUnreadNotificationCount(long? userId);
        GridResultVM<UserNotificationMainGridResultVM> GetList(UserNotificationMainGrid searchInput, long? userId, int? siteSettingId);
        object GetBy(string id, long? userId, int? siteSettingId);
    }
}
