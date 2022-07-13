using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.PaymentService.Models.DB
{
    [Table("TitakUsers")]
    public class TitakUser
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Username { get; set; }
        [Required, MaxLength(50)]
        public string Password { get; set; }
        public int SiteSettingId { get; set; }
    }
}
