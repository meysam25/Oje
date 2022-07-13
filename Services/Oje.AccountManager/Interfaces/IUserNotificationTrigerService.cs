using Oje.AccountService.Models.View;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Interfaces
{
    public interface IUserNotificationTrigerService
    {
        ApiResult Create(CreateUpdateUserNotificationTrigerVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Update(CreateUpdateUserNotificationTrigerVM input, int? siteSettingId);
        GridResultVM<UserNotificationTrigerMainGridResultVM> GetList(UserNotificationTrigerMainGrid searchInput, int? siteSettingId);
        void CreateNotificationForUser(long? userId, UserNotificationType type, List<PPFUserTypes> exteraUserList, long? objectId, string title, int? siteSettingId, string openLink, object exteraParameter);
    }
}
