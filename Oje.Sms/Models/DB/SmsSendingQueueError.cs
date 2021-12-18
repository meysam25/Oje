using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Sms.Models.DB
{
    [Table("SmsSendingQueueErrors")]
    public class SmsSendingQueueError
    {
        public long SmsSendingQueueId { get; set; }
        [ForeignKey("SmsSendingQueueId"), InverseProperty("SmsSendingQueueErrors")]
        public SmsSendingQueue SmsSendingQueue { get; set; }
        public DateTime CreateDate { get; set; }
        [Required, MaxLength(4000)]
        public string Description { get; set; }
        public int SmsConfigId { get; set; }
        [ForeignKey("SmsConfigId"), InverseProperty("SmsSendingQueueErrors")]
        public SmsConfig SmsConfig { get; set; }
    }
}
