using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.RegisterForm.Models.DB
{
    [Table("Roles")]
    public class Role
    {
        public Role()
        {
            UserRegisterFormRoles = new();
            UserRoles = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Title { get; set; }
        public int? SiteSettingId { get; set; }

        [InverseProperty("Role")]
        public List<UserRegisterFormRole> UserRegisterFormRoles { get; set; }
        [InverseProperty("Role")]
        public List<UserRole> UserRoles { get; set; }
    }
}
