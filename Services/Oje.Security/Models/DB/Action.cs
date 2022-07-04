using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Security.Models.DB
{
    [Table("Actions")]
    public class Action
    {
        public Action()
        {
            UserAdminLogConfigs = new();
            UserAdminLogs = new();
            AdminBlockClientConfigs = new();
        }

        [Key]
        public long Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        public int ControllerId { get; set; }
        [ForeignKey("ControllerId"), InverseProperty("Actions")]
        public Controller Controller { get; set; }

        [InverseProperty("Action")]
        public List<UserAdminLogConfig> UserAdminLogConfigs { get; set; }
        [InverseProperty("Action")]
        public List<UserAdminLog> UserAdminLogs { get; set; }
        [InverseProperty("Action")]
        public List<AdminBlockClientConfig> AdminBlockClientConfigs { get; set; }
    }
}
