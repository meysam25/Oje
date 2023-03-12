using Oje.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ValidatedSignature.Models.DB
{
    [Table("UserRegisterFormPrices")]
    public class UserRegisterFormPrice: SignatureEntity
    {
        [Key]
        public int Id { get; set; }
        public int UserRegisterFormId { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        public long Price { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(50)]
        public string GroupPriceTitle { get; set; }
        [MaxLength(50)]
        public string GroupPriceTitle2 { get; set; }
    }
}
