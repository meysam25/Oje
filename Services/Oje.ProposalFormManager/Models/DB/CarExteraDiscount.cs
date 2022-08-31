using Oje.Infrastructure.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("CarExteraDiscounts")]
    public class CarExteraDiscount
    {
        public CarExteraDiscount()
        {
            CarExteraDiscountValues = new List<CarExteraDiscountValue>();
            CarExteraDiscountRangeAmounts = new List<CarExteraDiscountRangeAmount>();
        }

        [Key]
        public int Id { get; set; }
        public int ProposalFormId { get; set; }
        [ForeignKey("ProposalFormId")]
        [InverseProperty("CarExteraDiscounts")]
        public ProposalForm ProposalForm { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        public bool IsOption { get; set; }
        public CarExteraDiscountType Type { get; set; }
        public int? VehicleTypeId { get; set; }
        [ForeignKey("VehicleTypeId"), InverseProperty("CarExteraDiscounts")]
        public VehicleType VehicleType { get; set; }
        public CarExteraDiscountCalculateType CalculateType { get; set; }
        public bool IsActive { get; set; }
        public bool? HasPrevInsurance { get; set; }
        [MaxLength(4000)]
        public string Description { get; set; }
        public int? CarExteraDiscountCategoryId { get; set; }
        [ForeignKey("CarExteraDiscountCategoryId"), InverseProperty("CarExteraDiscounts")]
        public CarExteraDiscountCategory CarExteraDiscountCategory { get; set; }
        public int? Order { get; set; }
        public bool? DontRemoveInSearch { get; set; }

        [InverseProperty("CarExteraDiscount")]
        public List<CarExteraDiscountValue> CarExteraDiscountValues { get; set; }
        [InverseProperty("CarExteraDiscount")]
        public List<CarExteraDiscountRangeAmount> CarExteraDiscountRangeAmounts { get; set; }
    }
}
