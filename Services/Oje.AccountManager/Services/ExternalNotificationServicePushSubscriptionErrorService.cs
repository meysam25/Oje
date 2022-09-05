using Microsoft.EntityFrameworkCore;
using Oje.AccountService.Interfaces;
using Oje.AccountService.Models.DB;
using Oje.AccountService.Services.EContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Services
{
    public class ExternalNotificationServicePushSubscriptionErrorService: IExternalNotificationServicePushSubscriptionErrorService
    {
        readonly AccountDBContext db = null;
        public ExternalNotificationServicePushSubscriptionErrorService
            (
                AccountDBContext db
            )
        {
            this.db = db;
        }

        public void Create(long endpointHash, DateTime now, string message, int ExternalNotificationServiceConfigId, int siteSettingId)
        {
            if (!string.IsNullOrEmpty(message) && ExternalNotificationServiceConfigId > 0 && siteSettingId > 0)
            {
                db.Entry(new ExternalNotificationServicePushSubscriptionError() 
                {
                    CreateDate = now,
                    EndpointHash = endpointHash,
                    ExternalNotificationServiceConfigId = ExternalNotificationServiceConfigId,
                    SiteSettingId = siteSettingId,
                    Message = message
                }).State = EntityState.Added;
                db.SaveChanges();
            }
        }
    }
}
