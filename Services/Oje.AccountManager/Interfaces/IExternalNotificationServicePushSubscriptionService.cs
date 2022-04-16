using Oje.AccountService.Models.View;

namespace Oje.AccountService.Interfaces
{
    public interface IExternalNotificationServicePushSubscriptionService
    {
        void Create(ExternalNotificationServicePushSubscriptionCreateVM input, long? userId, int? siteSettingId);
    }
}
