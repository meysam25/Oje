using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Models.DB
{
    [Table("ExternalNotificationServicePushSubscriptionErrors")]
    public class ExternalNotificationServicePushSubscriptionError
    {
        [Key]
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        public long EndpointHash { get; set; }
        [ForeignKey("EndpointHash"), InverseProperty("ExternalNotificationServicePushSubscriptionErrors")]
        public ExternalNotificationServicePushSubscription ExternalNotificationServicePushSubscription { get; set; }
        public int ExternalNotificationServiceConfigId { get; set; }
        [ForeignKey("ExternalNotificationServiceConfigId"), InverseProperty("ExternalNotificationServicePushSubscriptionErrors")]
        public ExternalNotificationServiceConfig ExternalNotificationServiceConfig { get; set; }
        public int SiteSettingId { get; set; }
    }
}
