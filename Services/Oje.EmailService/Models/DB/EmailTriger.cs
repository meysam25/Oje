using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Interfac;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.EmailService.Models.DB
{
    [Table("EmailTrigers")]
    public class EmailTriger: IEntityWithSiteSettingId
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
        [ForeignKey("SiteSettingId"), InverseProperty("EmailTrigers")]
        public SiteSetting SiteSetting { get; set; }
    }
}
