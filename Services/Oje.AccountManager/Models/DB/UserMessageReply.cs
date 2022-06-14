using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.AccountService.Models.DB
{
    [Table("UserMessageReplies")]
    public class UserMessageReply
    {
        [Key]
        public Guid Id { get; set; }
        public long UserMessageId { get; set; }
        [ForeignKey("UserMessageId"), InverseProperty("UserMessageReplies")]
        public UserMessage UserMessage { get; set; }
        public long UserId { get; set; }
        [ForeignKey("UserId"), InverseProperty("UserMessageReplies")]
        public User User { get; set; }
        public DateTime CreateDate { get; set; }
        [Required, MaxLength(4000)]
        public string Message { get; set; }
        [MaxLength(200)]
        public string FileUrl { get; set; }
    }
}
