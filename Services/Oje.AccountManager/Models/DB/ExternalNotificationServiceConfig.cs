using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Interfac;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.AccountService.Models.DB
{
    [Table("ExternalNotificationServiceConfigs")]
    public class ExternalNotificationServiceConfig: IEntityWithSiteSettingId
    {
        public ExternalNotificationServiceConfig()
        {
            ExternalNotificationServicePushSubscriptions = new();
            ExternalNotificationServicePushSubscriptionErrors = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Subject { get; set; }
        [Required, MaxLength(200)]
        public string PublicKey { get; set; }
        [Required, MaxLength(100)]
        public string PrivateKey { get; set; }
        public bool IsActive { get; set; }
        public ExternalNotificationServiceConfigType Type { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("ExternalNotificationServiceConfigs")]
        public SiteSetting SiteSetting { get; set; }

        [InverseProperty("ExternalNotificationServiceConfig")]
        public List<ExternalNotificationServicePushSubscription> ExternalNotificationServicePushSubscriptions { get; set; }
        [InverseProperty("ExternalNotificationServiceConfig")]
        public List<ExternalNotificationServicePushSubscriptionError> ExternalNotificationServicePushSubscriptionErrors { get; set; }
    }
}
