using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.AccountService.Models.DB
{
    [Table("LoginBackgroundImages")]
    public class LoginBackgroundImage
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        [Required, MaxLength(200)]
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
    }
}
