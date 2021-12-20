using Oje.AccountService.Models.DB;
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
    public interface IUserNotificationTemplateService
    {
        ApiResult Create(CreateUpdateUserNotificationTemplateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Update(CreateUpdateUserNotificationTemplateVM input, int? siteSettingId);
        GridResultVM<UserNotificationTemplateMainGridResultVM> GetList(UserNotificationTemplateMainGrid searchInput, int? siteSettingId);
        List<UserNotificationTemplate> GetBy(UserNotificationType type, int? siteSettingId);
    }
}
