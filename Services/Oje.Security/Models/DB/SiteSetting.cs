using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Security.Models.DB
{
    [Table("SiteSettings")]
    public class SiteSetting
    {
        public SiteSetting()
        {
            AdminBlockClientConfigs = new();
            BlockAutoIps = new();
            BlockClientConfigs = new();
            BlockLoginUsers = new();
            UserAdminLogs = new();
            UserAdminLogConfigs = new();
            UserLoginLogoutLogs = new();
        }

        [Key]
        public int Id { get; set; } 
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [InverseProperty("SiteSetting")]
        public List<AdminBlockClientConfig> AdminBlockClientConfigs { get; set; }
        [InverseProperty("SiteSetting")]
        public List<BlockAutoIp> BlockAutoIps { get; set; }
        [InverseProperty("SiteSetting")]
        public List<BlockClientConfig> BlockClientConfigs { get; set; }
        [InverseProperty("SiteSetting")]
        public List<BlockLoginUser> BlockLoginUsers { get; set; }
        [InverseProperty("SiteSetting")]
        public List<UserAdminLog> UserAdminLogs { get; set; }
        [InverseProperty("SiteSetting")]
        public List<UserAdminLogConfig> UserAdminLogConfigs { get; set; }
        [InverseProperty("SiteSetting")]
        public List<UserLoginLogoutLog> UserLoginLogoutLogs { get; set; }
    }
}
