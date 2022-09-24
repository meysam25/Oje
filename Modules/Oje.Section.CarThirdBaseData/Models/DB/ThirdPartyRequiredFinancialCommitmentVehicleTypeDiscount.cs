using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.CarThirdBaseData.Models.DB
{
    [Table("ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscounts")]
    public class ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscount
    {
        public ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscount()
        {
            ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCompanies = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        public long Price { get; set; }
        public int Percent { get; set; }
        public int VehicleTypeId { get; set; }
        [ForeignKey("VehicleTypeId"), InverseProperty("ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscounts")]
        public VehicleType VehicleType { get; set; }
        public int SiteSettingId { get; set; }

        [InverseProperty("ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscount")]
        public List<ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCompany> ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCompanies { get; set; }
    }
}
