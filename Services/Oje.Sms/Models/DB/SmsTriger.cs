using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Interfac;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Sms.Models.DB
{
    [Table("SmsTrigers")]
    public class SmsTriger: IEntityWithSiteSettingId
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
        [ForeignKey("SiteSettingId"), InverseProperty("SmsTrigers")]
        public SiteSetting SiteSetting { get; set; }
    }
}
