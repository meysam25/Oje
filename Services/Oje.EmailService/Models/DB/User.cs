using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.EmailService.Models.DB
{
    [Table("Users")]
    public class User
    {
        public User()
        {
            EmailTrigers = new();
            FromUserEmailSendingQueues = new();
            ToUserEmailSendingQueue = new();
            UserRoles = new ();
        }

        [Key]
        public long Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Username { get; set; }
        [Required]
        [MaxLength(50)]
        public string Firstname { get; set; }
        [Required]
        [MaxLength(50)]
        public string Lastname { get; set; }
        [MaxLength(14)]
        public string Mobile { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        public int? SiteSettingId { get; set; }

        [InverseProperty("User")]
        public List<EmailTriger> EmailTrigers { get; set; }
        [InverseProperty("FromUser")]
        public List<EmailSendingQueue> FromUserEmailSendingQueues { get; set; }
        [InverseProperty("ToUser")]
        public List<EmailSendingQueue> ToUserEmailSendingQueue { get; set; }
        [InverseProperty("User")]
        public List<UserRole> UserRoles { get; set; }
    }
}
