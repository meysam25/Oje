using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Sms.Models.DB
{
    [Table("SmsTrigers")]
    public class SmsTriger
    {
        [Key]
        public int Id { get; set; }
        public UserNotificationType UserNotificationType { get; set; }
        public int? RoleId { get; set; }
        [ForeignKey("RoleId"), InverseProperty("SmsTrigers")]
        public Role Role { get; set; }
        public long? UserId { get; set; }
        [ForeignKey("UserId"), InverseProperty("SmsTrigers")]
        public User User { get; set; }
        public int SiteSettingId { get; set; }
    }
}
