using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Secretariat.Models.DB
{
    [Table("Roles")]
    public class Role : SignatureEntity
    {
        public Role()
        {
            UserRoles = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Title { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        public long Value { get; set; }
        public bool? DisabledOnlyMyStuff { get; set; }
        public RoleType? Type { get; set; }
        public bool? RefreshGrid { get; set; }
        public int? SiteSettingId { get; set; }
        public bool? CanSeeOtherSites { get; set; }

        [InverseProperty(nameof(Role))]
        public List<UserRole> UserRoles { get; set; }
    }
}
