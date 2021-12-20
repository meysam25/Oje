using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.EmailService.Models.DB
{
    [Table("EmailConfigs")]
    public class EmailConfig
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

        [InverseProperty("EmailConfig")]
        public List<EmailSendingQueueError> EmailSendingQueueErrors { get; set; }
    }
}
