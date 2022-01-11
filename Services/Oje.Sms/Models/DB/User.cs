using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Sms.Models.DB
{
    [Table("Users")]
    public class User
    {
        public User()
        {
            SmsTrigers = new();
            FromUserSmsSendingQueues = new();
            ToUserSmsSendingQueues = new();
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
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int? SiteSettingId { get; set; }

        [InverseProperty("User")]
        public List<SmsTriger> SmsTrigers { get; set; }
        [InverseProperty("FromUser")]
        public List<SmsSendingQueue> FromUserSmsSendingQueues { get; set; }
        [InverseProperty("ToUser")]
        public List<SmsSendingQueue> ToUserSmsSendingQueues { get; set; }
        [InverseProperty("User")]
        public List<UserRole> UserRoles { get; set; }
    }
}
