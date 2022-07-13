using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.RegisterForm.Models.DB
{
    [Table("UserRegisterFormPrices")]
    public class UserRegisterFormPrice
    {
        [Key]
        public int Id { get; set; }
        public int UserRegisterFormId { get; set; }
        [ForeignKey("UserRegisterFormId"), InverseProperty("UserRegisterFormPrices")]
        public UserRegisterForm UserRegisterForm { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        public long Price { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(50)]
        public string GroupPriceTitle { get; set; }
    }
}
