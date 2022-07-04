using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Sanab.Models.DB
{
    [Table("SanabUsers")]
    public class SanabUser
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Username { get; set; }
        [Required, MaxLength(100)]
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
    }
}
