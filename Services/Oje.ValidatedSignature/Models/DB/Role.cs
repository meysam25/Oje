using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ValidatedSignature.Models.DB
{
    [Table("Roles")]
    public class Role: SignatureEntity
    {
        public Role()
        {
            UserRoles = new();
            RoleActions = new();
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
    }
}
