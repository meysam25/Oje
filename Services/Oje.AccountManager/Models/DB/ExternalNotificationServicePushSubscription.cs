using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.AccountService.Models.DB
{
    [Table("ExternalNotificationServicePushSubscriptions")]
    public class ExternalNotificationServicePushSubscription
    {
        public ExternalNotificationServicePushSubscription()
        {
            ExternalNotificationServicePushSubscriptionErrors = new();
        }

        [Key]
        public long EndpointHash { get; set; }
        public int ExternalNotificationServiceConfigId { get; set; }
        [ForeignKey("ExternalNotificationServiceConfigId"), InverseProperty("ExternalNotificationServicePushSubscriptions")]
        public ExternalNotificationServiceConfig ExternalNotificationServiceConfig { get; set; }
        public DateTime CreateDate { get; set; }
        [Required, MaxLength(100)]
        public string Auth { get; set; }
        [Required, MaxLength(200)]
        public string P256DH { get; set; }
        [Required, MaxLength(1000)]
        public string Endpoint { get; set; }
        public bool IsActive { get; set; }
        public long? UserId { get; set; }
        [ForeignKey("UserId"), InverseProperty("ExternalNotificationServicePushSubscriptions")]
        public User User { get; set; }
        public int SiteSettingId { get; set; }

        [InverseProperty("ExternalNotificationServicePushSubscription")]
        public List<ExternalNotificationServicePushSubscriptionError> ExternalNotificationServicePushSubscriptionErrors { get; set; }
    }
}
