using Oje.Infrastructure.Interfac;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.EmailService.Models.DB
{
    [Table("EmailConfigs")]
    public class EmailConfig: IEntityWithSiteSettingId
    {
        public EmailConfig()
        {
            EmailSendingQueueErrors = new();
        }

        [Key]
        public int Id { get; set; }
        [Required,MaxLength(100)]
        public string Title { get; set; }
        [Required, MaxLength(100)]
        public string Username { get; set; }
        [Required,MaxLength(100)]
        public string Password { get; set; }
        public int SmtpPort { get; set; }
        [Required,MaxLength (100)]
        public string SmtpHost { get; set; }
        public bool EnableSsl { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public int Timeout { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("EmailConfigs")]
        public SiteSetting SiteSetting { get; set; }

        [InverseProperty("EmailConfig")]
        public List<EmailSendingQueueError> EmailSendingQueueErrors { get; set; }
    }
}
