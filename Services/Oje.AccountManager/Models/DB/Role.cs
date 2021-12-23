using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Models.DB
{
    [Table("Roles")]
    public class Role
    {
        public Role()
        {
            UserRoles = new();
            RoleActions = new();
            RoleProposalForms = new();
            UserNotificationTrigers = new();
        }

        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string Title { get; set; }
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        public long Value { get; set; }
        public bool? DisabledOnlyMyStuff { get; set; }
        public RoleType? Type { get; set; }
        public int? SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId")]
        [InverseProperty("Roles")]
        public SiteSetting SiteSetting { get; set; }

        [InverseProperty("Role")]
        public List<RoleAction> RoleActions { get; set; }
        [InverseProperty("Role")]
        public List<UserRole> UserRoles { get; set; }
        [InverseProperty("Role")]
        public List<RoleProposalForm> RoleProposalForms { get; set; }
        [InverseProperty("Role")]
        public List<UserNotificationTriger> UserNotificationTrigers { get; set; }
    }
}
