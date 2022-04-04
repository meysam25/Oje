using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.InsuranceContractBaseData.Models.DB
{
    [Table("InsuranceContractProposalFilledFormUsers")]
    public class InsuranceContractProposalFilledFormUser
    {
        public long InsuranceContractProposalFilledFormId { get; set; }
        [ForeignKey("InsuranceContractProposalFilledFormId"), InverseProperty("InsuranceContractProposalFilledFormUsers")]
        public InsuranceContractProposalFilledForm InsuranceContractProposalFilledForm { get; set; }
        public long InsuranceContractUserId { get; set; }
        [ForeignKey("InsuranceContractUserId"), InverseProperty("InsuranceContractProposalFilledFormUsers")]
        public InsuranceContractUser InsuranceContractUser { get; set; }
        public int InsuranceContractTypeId { get; set; }
        [ForeignKey("InsuranceContractTypeId"), InverseProperty("InsuranceContractProposalFilledFormUsers")]
        public InsuranceContractType InsuranceContractType { get; set; }
    }
}
