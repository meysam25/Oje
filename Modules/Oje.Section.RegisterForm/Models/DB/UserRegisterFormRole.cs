using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.RegisterForm.Models.DB
{
    [Table("UserRegisterFormRoles")]
    public class UserRegisterFormRole
    {
        public int UserRegisterFormId { get; set; }
        [ForeignKey("UserRegisterFormId"), InverseProperty("UserRegisterFormRoles")]
        public UserRegisterForm UserRegisterForm { get; set; }
        public int RoleId { get; set; }
        [ForeignKey("RoleId"), InverseProperty("UserRegisterFormRoles")]
        public Role Role { get; set; }
    }
}
