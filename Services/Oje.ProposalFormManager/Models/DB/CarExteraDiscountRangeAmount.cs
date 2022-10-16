using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("CarExteraDiscountRangeAmounts")]
    public class CarExteraDiscountRangeAmount
    {
        public CarExteraDiscountRangeAmount()
        {
            CarExteraDiscountRangeAmountCompanies = new List<CarExteraDiscountRangeAmountCompany>();
        }

        [Key]
        public int Id { get; set; }
        public int CarExteraDiscountId { get; set; }
        [ForeignKey("CarExteraDiscountId")]
        [InverseProperty("CarExteraDiscountRangeAmounts")]
        public CarExteraDiscount CarExteraDiscount { get; set; }
        public int? CarExteraDiscountValueId { get; set; }
        [ForeignKey("CarExteraDiscountValueId")]
        [InverseProperty("CarExteraDiscountRangeAmounts")]
        public CarExteraDiscountValue CarExteraDiscountValue { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        public long MinValue { get; set; }
        public long MaxValue { get; set; }
        public decimal? Percent { get; set; }
        public long? Amount { get; set; }
        public bool IsActive { get; set; }
        public decimal? CreateDateSelfPercent { get; set; }

        [InverseProperty("CarExteraDiscountRangeAmount")]
        public List<CarExteraDiscountRangeAmountCompany> CarExteraDiscountRangeAmountCompanies { get; set; }
    }
}
