using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Interfac;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Sms.Models.DB
{
    [Table("SmsConfigs")]
    public class SmsConfig: IEntityWithSiteSettingId
    {
        public SmsConfig()
        {
            SmsSendingQueueErrors = new();
        }

        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Username { get; set; }
        [MaxLength(4000)]
        public string Password { get; set; }
        public string Domain { get; set; }
        public SmsConfigType Type { get; set; }
        [Required, MaxLength(20)]
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("SmsConfigs")]
        public SiteSetting SiteSetting { get; set; }

        [InverseProperty("SmsConfig")]
        public List<SmsSendingQueueError> SmsSendingQueueErrors { get; set; }
    }
}
