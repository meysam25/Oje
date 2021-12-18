using Oje.AccountService.Models.DB;
using Oje.AccountService.Models.View;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
