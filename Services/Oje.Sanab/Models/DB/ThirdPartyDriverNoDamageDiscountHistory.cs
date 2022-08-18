using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Sanab.Models.DB
{
    [Table("ThirdPartyDriverNoDamageDiscountHistories")]
    public class ThirdPartyDriverNoDamageDiscountHistory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public int Percent { get; set; }
        public bool IsActive { get; set; }
    }
}
