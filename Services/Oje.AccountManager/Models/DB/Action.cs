using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.AccountService.Models.DB
{
    [Table("Actions")]
    public class Action
    {
        public Action()
        {
            RoleActions = new ();
            UserAdminLogConfigs = new();
            UserAdminLogs = new();
        }

        [Key]
        public long Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(30)]
        public string Icon { get; set; }
        [Required]
        public int ControllerId { get; set; }
        [ForeignKey("ControllerId")]
        [InverseProperty("Actions")]
        public Controller Controller { get; set; }
        public bool IsMainMenuItem { get; set; }

        [InverseProperty("Action")]
        public List<RoleAction> RoleActions { get; set; }
        [InverseProperty("Action")]
        public List<DashboardSection> DashboardSections { get; set; }
        [InverseProperty("Action")]
        public List<UserAdminLogConfig> UserAdminLogConfigs { get; set; }
        [InverseProperty("Action")]
        public List<UserAdminLog> UserAdminLogs { get; set; }
    }
}
