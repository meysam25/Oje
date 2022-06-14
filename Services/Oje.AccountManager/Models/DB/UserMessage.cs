using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.AccountService.Models.DB
{
    [Table("UserMessages")]
    public class UserMessage
    {
        public UserMessage()
        {
            UserMessageReplies = new();
        }

        [Key]
        public long Id { get; set; }
        public long FromUserId { get; set; }
        [ForeignKey("FromUserId"), InverseProperty("FromUserUserMessages")]
        public User FromUser { get; set; }
        public long ToUserId { get; set; }
        [ForeignKey("ToUserId"), InverseProperty("ToUserUserMessages")]
        public User ToUser { get; set; }
        public DateTime CreateDate { get; set; }
        public int SiteSettingId { get; set; }

        [InverseProperty("UserMessage")]
        public List<UserMessageReply> UserMessageReplies { get; set; }
    }
}
