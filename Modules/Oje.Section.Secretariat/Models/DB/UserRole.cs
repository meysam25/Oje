using Oje.Infrastructure.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.Secretariat.Models.DB
{
    [Table("UserRoles")]
    public class UserRole: SignatureEntity
    {
        [Key]
        public Guid Id { get; set; }
        public long UserId { get; set; }
        [ForeignKey(nameof(UserId)), InverseProperty("UserRoles")]
        public User User { get; set; }
        public int RoleId { get; set; }
        [ForeignKey(nameof(RoleId)), InverseProperty("UserRoles")]
        public Role Role { get; set; }
    }
}
