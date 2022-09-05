using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.AccountService.Models.DB
{
    [Table("OurObjects")]
    public class OurObject
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(200)]
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public int SiteSettingId { get; set; }
    }
}
