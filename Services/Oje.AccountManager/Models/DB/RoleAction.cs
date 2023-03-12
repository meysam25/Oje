using Oje.Infrastructure.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.AccountService.Models.DB
{
    [Table("RoleActions")]
    public class RoleAction: SignatureEntity
    {
        public RoleAction()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        [InverseProperty("RoleActions")]
        public Role Role { get; set; }
        public long ActionId { get; set; }
        [ForeignKey("ActionId")]
        [InverseProperty("RoleActions")]
        public Action Action { get; set; }
    }
}
