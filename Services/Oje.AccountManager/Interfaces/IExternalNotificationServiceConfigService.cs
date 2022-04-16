using Oje.AccountService.Models.DB;
using Oje.AccountService.Models.View;
using Oje.Infrastructure.Models;
using System.Collections.Generic;

namespace Oje.AccountService.Interfaces
{
    public interface IExternalNotificationServiceConfigService
    {
        ApiResult Create(ExternalNotificationServiceConfigCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        ExternalNotificationServiceConfigCreateUpdateVM GetById(int? id, int? siteSettingId);
        ApiResult Update(ExternalNotificationServiceConfigCreateUpdateVM input, int? siteSettingId);
        GridResultVM<ExternalNotificationServiceConfigMainGridResultVM> GetList(ExternalNotificationServiceConfigMainGrid searchInput, int? siteSettingId);
        ExternalNotificationServiceConfig GetActiveConfig(int? siteSettingId);
        List<ExternalNotificationServiceConfig> GetActiveConfig();
    }
}
