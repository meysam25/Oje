using Microsoft.EntityFrameworkCore;
using Oje.AccountService.Interfaces;
using Oje.AccountService.Models.View;
using Oje.AccountService.Services.EContext;
using Oje.Infrastructure.Services;
using System;
using System.Linq;

namespace Oje.AccountService.Services
{
    public class ExternalNotificationServicePushSubscriptionService : IExternalNotificationServicePushSubscriptionService
    {
        readonly AccountDBContext db = null;
        readonly IExternalNotificationServiceConfigService ExternalNotificationServiceConfigService = null;

        static object lockObj = null;

        public ExternalNotificationServicePushSubscriptionService
            (
                AccountDBContext db,
                IExternalNotificationServiceConfigService ExternalNotificationServiceConfigService
            )
        {
            this.db = db;
            this.ExternalNotificationServiceConfigService = ExternalNotificationServiceConfigService;
        }

        public void Create(ExternalNotificationServicePushSubscriptionCreateVM input, long? userId, int? siteSettingId)
        {
            if (input != null && !string.IsNullOrEmpty(input.auth) && !string.IsNullOrEmpty(input.p256DH) && !string.IsNullOrEmpty(input.endpoint) && input.auth.Length <= 100 && input.p256DH.Length <= 200 && input.endpoint.Length <= 1000 && siteSettingId.ToIntReturnZiro() > 0)
            {
                if (lockObj == null)
                    lockObj = new();

                var foundConfig = ExternalNotificationServiceConfigService.GetActiveConfig(siteSettingId);

                if (foundConfig != null)
                {
                    var itemId = (input.auth + input.p256DH + input.endpoint).GetHashCode32();
                    lock (lockObj)
                    {
                        if (!db.ExternalNotificationServicePushSubscriptions.Any(t => t.EndpointHash == itemId))
                        {
                            var foundItem = new Models.DB.ExternalNotificationServicePushSubscription()
                            {
                                SiteSettingId = siteSettingId.Value,
                                CreateDate = DateTime.Now,
                                ExternalNotificationServiceConfigId = foundConfig.Id,
                                IsActive = true,
                                EndpointHash = itemId,
                                Auth = input.auth,
                                P256DH = input.p256DH,
                                Endpoint = input.endpoint,
                                UserId = userId
                            };
                            db.Entry(foundItem).State = EntityState.Added;
                            db.SaveChanges();
                        }
                        else if (userId.ToLongReturnZiro() > 0)
                        {
                            var foundItem = db.ExternalNotificationServicePushSubscriptions.FirstOrDefault(t => t.EndpointHash == itemId);
                            if (foundItem != null)
                            {
                                foundItem.UserId = userId.ToLongReturnZiro();
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }
        }
    }
}
