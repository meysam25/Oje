using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Models.DB
{
    [Table("InsuranceContractProposalFilledFormValues")]
    public class InsuranceContractProposalFilledFormValue
    {
        [Key]
        public Guid Id { get; set; }
        public long InsuranceContractProposalFilledFormId { get; set; }
        [ForeignKey("InsuranceContractProposalFilledFormId"), InverseProperty("InsuranceContractProposalFilledFormValues")]
        public InsuranceContractProposalFilledForm InsuranceContractProposalFilledForm { get; set; }
        public int InsuranceContractProposalFilledFormKeyId { get; set; }
        [ForeignKey("InsuranceContractProposalFilledFormKeyId"), InverseProperty("InsuranceContractProposalFilledFormValues")]
        public InsuranceContractProposalFilledFormKey InsuranceContractProposalFilledFormKey { get; set; }
        [Required, MaxLength(4000)]
        public string Value { get; set; }
    }
}
