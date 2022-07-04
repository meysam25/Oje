using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.AccountService.Models.DB
{
    [Table("UserAdminLogConfigs")]
    public class UserAdminLogConfig
    {
        [Key]
        public long Id { get; set; }
        public long ActionId { get; set; }
        [ForeignKey("ActionId"), InverseProperty("UserAdminLogConfigs")]
        public Action Action { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
    }
}
