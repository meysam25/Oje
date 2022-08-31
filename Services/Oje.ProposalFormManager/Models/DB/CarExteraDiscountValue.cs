using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("CarExteraDiscountValues")]
    public class CarExteraDiscountValue
    {
        public CarExteraDiscountValue()
        {
            CarExteraDiscountRangeAmounts = new List<CarExteraDiscountRangeAmount>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        public int CarExteraDiscountId { get; set; }
        [ForeignKey("CarExteraDiscountId")]
        [InverseProperty("CarExteraDiscountValues")]
        public CarExteraDiscount CarExteraDiscount { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("CarExteraDiscountValue")]
        public List<CarExteraDiscountRangeAmount> CarExteraDiscountRangeAmounts { get; set; }
    }
}
