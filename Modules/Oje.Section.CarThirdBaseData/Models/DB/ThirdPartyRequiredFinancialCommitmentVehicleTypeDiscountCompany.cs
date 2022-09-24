using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.CarThirdBaseData.Models.DB
{
    [Table("ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCompanies")]
    public class ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCompany
    {
        public int ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountId { get; set; }
        [ForeignKey("ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountId"), InverseProperty("ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCompanies")]
        public ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscount ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscount { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId"), InverseProperty("ThirdPartyRequiredFinancialCommitmentVehicleTypeDiscountCompanies")]
        public Company Company { get; set; }
    }
}
