using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.InsuranceContractBaseData.Models.DB
{
    [Table("InsuranceContractInsuranceContractTypeMaxPrices")]
    public class InsuranceContractInsuranceContractTypeMaxPrice
    {
        public int InsuranceContractId { get; set; }
        [ForeignKey("InsuranceContractId"), InverseProperty("InsuranceContractInsuranceContractTypeMaxPrices")]
        public InsuranceContract InsuranceContract { get; set; }
        public int InsuranceContractTypeId { get; set; }
        [ForeignKey("InsuranceContractTypeId"), InverseProperty("InsuranceContractInsuranceContractTypeMaxPrices")]
        public InsuranceContractType InsuranceContractType { get; set; }
        public long MaxPrice { get; set; }
        public int SiteSettingId { get; set; }
    }
}
