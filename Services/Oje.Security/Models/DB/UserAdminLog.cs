using Oje.Infrastructure.Interfac;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Security.Models.DB
{
    [Table("UserAdminLogs")]
    public class UserAdminLog: IEntityWithSiteSettingId
    {
        [Key]
        public long Id { get; set; }
        public long UserId { get; set; }
        [ForeignKey("UserId"), InverseProperty("UserAdminLogs")]
        public User User { get; set; }
        public string RequestId { get; set; }
        public long ActionId { get; set; }
        [ForeignKey("ActionId"), InverseProperty("UserAdminLogs")]
        public Action Action { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsStart { get; set; }
        public byte Ip1 { get; set; }
        public byte Ip2 { get; set; }
        public byte Ip3 { get; set; }
        public byte Ip4 { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        public int SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId"), InverseProperty("UserAdminLogs")]
        public SiteSetting SiteSetting { get; set; }
    }
}
