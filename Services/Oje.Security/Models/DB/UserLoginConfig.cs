using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Security.Models.DB
{
    [Table("UserLoginConfigs")]
    public class UserLoginConfig
    {
        [Key]
        public int Id { get; set; }
        public int FailCount { get; set; }
        public int DeactiveMinute { get; set; }
        public int InActiveLogoffMinute { get; set; }
        public int SiteSettingId { get; set; }
    }
}
