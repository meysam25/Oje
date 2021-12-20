using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.EmailService.Models.DB
{
    [Table("EmailSendingQueues")]
    public class EmailSendingQueue
    {
        public EmailSendingQueue()
        {
            EmailSendingQueueErrors = new();
        }

        [Key]
        public long Id { get; set; }
        public UserNotificationType Type { get; set; }
        [Required,MaxLength(100)]
        public string Email { get; set; }
        [Required, MaxLength(50)]
        public string Subject { get; set; }
        [Required, MaxLength(4000)]
        public string Body { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastTryDate { get; set; }
        public int CountTry { get; set; }
        public long? FromUserId { get; set; }
        [ForeignKey("FromUserId"), InverseProperty("FromUserEmailSendingQueues")]
        public User FromUser { get; set; }
        public long? ToUserId { get; set; }
        [ForeignKey("ToUserId"), InverseProperty("ToUserEmailSendingQueue")]
        public User ToUser { get; set; }
        public long? ObjectId { get; set; }
        public bool IsSuccess { get; set; }
        public byte Ip1 { get; set; }
        public byte Ip2 { get; set; }
        public byte Ip3 { get; set; }
        public byte Ip4 { get; set; }
        public int SiteSettingId { get; set; }


        [InverseProperty("EmailSendingQueue")]
        public List<EmailSendingQueueError> EmailSendingQueueErrors { get; set; }
    }
}
