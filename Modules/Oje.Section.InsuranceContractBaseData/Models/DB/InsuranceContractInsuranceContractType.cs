using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.InsuranceContractBaseData.Models.DB
{
    [Table("InsuranceContractInsuranceContractTypes")]
    public class InsuranceContractInsuranceContractType
    {
        public int InsuranceContractId { get; set; }
        [ForeignKey("InsuranceContractId"), InverseProperty("InsuranceContractInsuranceContractTypes")]
        public InsuranceContract InsuranceContract { get; set; }
        public int InsuranceContractTypeId { get; set; }
        [ForeignKey("InsuranceContractTypeId"), InverseProperty("InsuranceContractInsuranceContractTypes")]
        public InsuranceContractType InsuranceContractType { get; set; }
    }
}
