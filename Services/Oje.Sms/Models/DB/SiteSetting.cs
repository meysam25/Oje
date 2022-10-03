using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Oje.Sms.Models.DB
{
    [Table("SiteSettings")]
    public class SiteSetting
    {
        public SiteSetting()
        {
            SmsConfigs = new();
            SmsSendingQueues = new();
            SmsTemplates = new();
            SmsTrigers = new();
            SmsValidationHistories = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [InverseProperty("SiteSetting")]
        public List<SmsConfig> SmsConfigs { get; set; }
        [InverseProperty("SiteSetting")]
        public List<SmsSendingQueue> SmsSendingQueues { get; set; }
        [InverseProperty("SiteSetting")]
        public List<SmsTemplate> SmsTemplates { get; set; }
        [InverseProperty("SiteSetting")]
        public List<SmsTriger> SmsTrigers { get; set; }
        [InverseProperty("SiteSetting")]
        public List<SmsValidationHistory> SmsValidationHistories { get; set; }
    }
}
