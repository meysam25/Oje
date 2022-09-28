using Oje.Infrastructure.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
            DashboardSections = new();
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
        public bool? RefreshGrid { get; set; }
        public int? SiteSettingId { get; set; }
        [ForeignKey("SiteSettingId")]
        [InverseProperty("Roles")]
        public SiteSetting SiteSetting { get; set; }
        public bool? CanSeeOtherSites { get; set; }

        [InverseProperty("Role")]
        public List<RoleAction> RoleActions { get; set; }
        [InverseProperty("Role")]
        public List<UserRole> UserRoles { get; set; }
        [InverseProperty("Role")]
        public List<RoleProposalForm> RoleProposalForms { get; set; }
        [InverseProperty("Role")]
        public List<UserNotificationTriger> UserNotificationTrigers { get; set; }
        [InverseProperty("Role")]
        public List<DashboardSection> DashboardSections { get; set; }
    }
}
