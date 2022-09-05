using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oje.AccountService.Interfaces;
using Oje.AccountService.Models.DB;
using Oje.AccountService.Services.EContext;
using Oje.Infrastructure.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using UAParser;
using WebPush;

namespace Oje.AccountService.Services
{
    public class PushNotificationService : IPushNotificationService
    {
        readonly AccountDBContext db = null;
        readonly IExternalNotificationServiceConfigService ExternalNotificationServiceConfigService = null;
        readonly IExternalNotificationServicePushSubscriptionErrorService ExternalNotificationServicePushSubscriptionErrorService = null;
        public PushNotificationService
            (
                AccountDBContext db,
                IExternalNotificationServiceConfigService ExternalNotificationServiceConfigService,
                IExternalNotificationServicePushSubscriptionErrorService ExternalNotificationServicePushSubscriptionErrorService
            )
        {
            this.db = db;
            this.ExternalNotificationServiceConfigService = ExternalNotificationServiceConfigService;
            this.ExternalNotificationServicePushSubscriptionErrorService = ExternalNotificationServicePushSubscriptionErrorService;
        }

        public async Task<ApiResult> Send(ExternalNotificationServiceConfig config, object input, ExternalNotificationServicePushSubscription device)
        {
            var webPushClient = new WebPushClient();
            var vapidDetails = new VapidDetails(config.Subject, config.PublicKey, config.PrivateKey);
            try
            {
                await webPushClient.SendNotificationAsync(new PushSubscription()
                {
                    Auth = device.Auth,
                    Endpoint = device.Endpoint,
                    P256DH = device.P256DH
                }, JsonConvert.SerializeObject(input), vapidDetails);

                return ApiResult.GenerateNewResult(true, Infrastructure.Exceptions.BMessages.Operation_Was_Successfull);
            }
            catch (Exception ex)
            {
                return new ApiResult() { isSuccess = false, message = ex.Message };
            }
        }

        public async Task SendWebNotifications()
        {
            var curDT = DateTime.Now.AddSeconds(-55);
            var allItems = db.UserNotifications
                .Include(t => t.User)
                .ThenInclude(t => t.ExternalNotificationServicePushSubscriptions)
                .Where(t => t.LastTryDate == null && (t.IsSuccess == false || t.IsSuccess == null) && (t.CountTry == null || t.CountTry == 0) && t.User.ExternalNotificationServicePushSubscriptions.Any(tt => tt.IsActive == true))
                .ToList();
            if (allItems.Count == 0)
                allItems = db.UserNotifications
                    .Include(t => t.User)
                    .ThenInclude(t => t.ExternalNotificationServicePushSubscriptions)
                    .Where(t => (t.IsSuccess == null || t.IsSuccess == false) && t.LastTryDate != null && curDT > t.LastTryDate && (t.CountTry == null || t.CountTry <= 2) && t.User.ExternalNotificationServicePushSubscriptions.Any(tt => tt.IsActive == true))
                    .ToList();
            foreach (var item in allItems)
            {
                item.LastTryDate = DateTime.Now;
                if (item.CountTry == null)
                    item.CountTry = 0;
                item.CountTry++;
            }

            db.SaveChanges();

            var foundConfigs = ExternalNotificationServiceConfigService.GetActiveConfig();

            foreach (var item in allItems)
            {
                var foundConfig = foundConfigs.Where(t => t.SiteSettingId == item.SiteSettingId).FirstOrDefault();
                if (foundConfig == null)
                    continue;

                var allDevices = item.User?.ExternalNotificationServicePushSubscriptions;
                if (allDevices == null || allDevices.Count == 0)
                    continue;

                foreach (var device in allDevices)
                {
                    ApiResult resultNotificaiton = null;
                    try
                    {
                        resultNotificaiton = await Send(foundConfig, new { title = item.Subject, body = item.Description, url = item.TargetPageLink }, device);
                    }
                    catch (Exception ex)
                    {
                        item.IsSuccess = false;
                        ExternalNotificationServicePushSubscriptionErrorService.Create(device.EndpointHash, DateTime.Now, ex.Message, foundConfig.Id, item.SiteSettingId);
                        device.removeMe = true;
                        continue;
                    };
                    if (resultNotificaiton != null && resultNotificaiton.isSuccess == true)
                    {
                        item.IsSuccess = true;
                    }
                    else if (resultNotificaiton != null)
                    {
                        item.IsSuccess = false;
                        ExternalNotificationServicePushSubscriptionErrorService.Create(device.EndpointHash, DateTime.Now, resultNotificaiton.message, foundConfig.Id, item.SiteSettingId);
                        device.removeMe = true;
                    }
                    else
                    {
                        item.IsSuccess = false;
                        ExternalNotificationServicePushSubscriptionErrorService.Create(device.EndpointHash, DateTime.Now, "علت خطا مشخص نمی باشد", foundConfig.Id, item.SiteSettingId);
                        device.removeMe = true;
                    }
                }
            }

            var allItemNeedToBeRemoved = allItems
                .Where(t => t.User != null)
                .Select(t => t.User)
                .Where(t => t.ExternalNotificationServicePushSubscriptions != null && t.ExternalNotificationServicePushSubscriptions.Count > 0)
                .SelectMany(t => t.ExternalNotificationServicePushSubscriptions)
                .Where(t => t.removeMe == true)
                .ToList();
            if (allItemNeedToBeRemoved != null)
                for (var i = 0; i < allItemNeedToBeRemoved.Count; i++)
                    db.Entry(allItemNeedToBeRemoved[i]).State = EntityState.Deleted;

            db.SaveChanges();
        }
    }
}
