using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Security.Models.DB
{
    [Table("UserAdminLogConfigs")]
    public class UserAdminLogConfig
    {
        [Key]
        public long Id { get; set; }
        public long ActionId { get; set; }
        [ForeignKey("ActionId"), InverseProperty("UserAdminLogConfigs")]
        public DB.Action Action { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
    }
}
