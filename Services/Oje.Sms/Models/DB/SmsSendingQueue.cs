﻿using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Interfac;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Sms.Models.DB
{
    [Table("SmsSendingQueues")]
    public class SmsSendingQueue: IEntityWithSiteSettingId
    {

        public SmsSendingQueue()
        {
            SmsSendingQueueErrors = new();
        }

        [Key]
        public long Id { get; set; }
        public UserNotificationType Type { get; set; }
        [Required, MaxLength(20)]
        public string MobileNumber { get; set; }
        [Required, MinLength(50)]
        public string Subject { get; set; }
        [Required, MaxLength(4000)]
        public string Body { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastTryDate { get; set; }
        public int CountTry { get; set; }
        public long? FromUserId { get; set; }
        [ForeignKey("FromUserId"), InverseProperty("FromUserSmsSendingQueues")]
        public User FromUser { get; set; }
        public long? ToUserId { get; set; }
        [ForeignKey("ToUserId"), InverseProperty("ToUserSmsSendingQueues")]
        public User ToUser { get; set; }
        public long? ObjectId { get; set; }
        public bool IsSuccess { get; set; }
        public byte Ip1 { get; set; }
        public byte Ip2 { get; set; }
        public byte Ip3 { get; set; }
        public byte Ip4 { get; set; }
        [MaxLength(50)]
        public string TraceCode { get; set; }
        public long? CreateUserId { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("SmsSendingQueues")]
        public SiteSetting SiteSetting { get; set; }

        [InverseProperty("SmsSendingQueue")]
        public List<SmsSendingQueueError> SmsSendingQueueErrors { get; set; }

    }
}
