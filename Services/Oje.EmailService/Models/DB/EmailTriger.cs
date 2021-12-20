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
    [Table("EmailTrigers")]
    public class EmailTriger
    {
        [Key]
        public int Id { get; set; }
        public UserNotificationType Type { get; set; }
        public int? RoleId { get; set; }
        [ForeignKey("RoleId"), InverseProperty("EmailTrigers")]
        public Role Role { get; set; }
        public long? UserId { get; set; }
        [ForeignKey("UserId"), InverseProperty("EmailTrigers")]
        public User User { get; set; }
        public int SiteSettingId { get; set; }
    }
}
