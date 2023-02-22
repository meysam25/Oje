using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.RegisterForm.Models.DB
{
    [Table("UserRegisterFormCategories")]
    public class UserRegisterFormCategory
    {
        public UserRegisterFormCategory()
        {
            UserRegisterForms = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        [Required, MaxLength(50)]
        public string Icon { get; set; }
        public int SiteSettingId { get; set; }

        [InverseProperty("UserRegisterFormCategory")]
        public List<UserRegisterForm> UserRegisterForms { get; set; }
    }
}
