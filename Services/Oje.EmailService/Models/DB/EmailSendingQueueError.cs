using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.EmailService.Models.DB
{
    [Table("EmailSendingQueueErrors")]
    public class EmailSendingQueueError
    {
        public long EmailSendingQueueId { get; set; }
        [ForeignKey("EmailSendingQueueId"), InverseProperty("EmailSendingQueueErrors")]
        public EmailSendingQueue EmailSendingQueue { get; set; }
        public DateTime CreateDate { get; set; }
        [Required, MaxLength(4000)]
        public string Description { get; set; }
        public int? EmailConfigId { get; set; }
        [ForeignKey("EmailConfigId"), InverseProperty("EmailSendingQueueErrors")]
        public EmailConfig EmailConfig { get; set; }
    }
}
