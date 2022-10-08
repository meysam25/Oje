using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.EmailService.Models.DB
{
    [Table("SiteSettings")]
    public class SiteSetting
    {
        public SiteSetting()
        {
            EmailConfigs = new();
            EmailSendingQueues = new();
            EmailTemplates = new();
            EmailTrigers = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [InverseProperty("SiteSetting")]
        public List<EmailConfig> EmailConfigs { get; set; }
        [InverseProperty("SiteSetting")]
        public List<EmailSendingQueue> EmailSendingQueues { get; set; }
        [InverseProperty("SiteSetting")]
        public List<EmailTemplate> EmailTemplates { get; set; }
        [InverseProperty("SiteSetting")]
        public List<EmailTriger> EmailTrigers { get; set; }
    }
}
