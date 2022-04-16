using Oje.AccountService.Models.DB;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Interfaces
{
    public interface IPushNotificationService
    {
        Task<ApiResult> Send(ExternalNotificationServiceConfig config, object input, ExternalNotificationServicePushSubscription device);
        Task SendWebNotifications();
    }
}
