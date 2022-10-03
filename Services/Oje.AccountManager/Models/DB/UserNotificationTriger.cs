using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Interfac;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.AccountService.Models.DB
{
    [Table("UserNotificationTrigers")]
    public class UserNotificationTriger: IEntityWithSiteSettingId
    {
        [Key]
        public int Id { get; set; }
        public UserNotificationType UserNotificationType { get; set; }
        public int? RoleId { get; set; }
        [ForeignKey("RoleId"), InverseProperty("UserNotificationTrigers")]
        public Role Role { get; set; }
        public long? UserId { get; set; }
        [ForeignKey("UserId"), InverseProperty("UserNotificationTrigers")]
        public User User { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("UserNotificationTrigers")]
        public SiteSetting SiteSetting { get; set; }
    }
}
