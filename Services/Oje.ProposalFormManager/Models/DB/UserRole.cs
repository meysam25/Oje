using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("UserRoles")]
    public class UserRole
    {
        [Key]
        public Guid Id { get; set; }
        public long UserId { get; set; }
        [ForeignKey("UserId"), InverseProperty("UserRoles")]
        public User User { get; set; }
        public int RoleId { get; set; }
    }
}
