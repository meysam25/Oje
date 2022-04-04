using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.InsuranceContractBaseData.Models.DB
{
    [Table("InsuranceContractProposalFilledFormKeys")]
    public class InsuranceContractProposalFilledFormKey
    {
        public InsuranceContractProposalFilledFormKey()
        {
            InsuranceContractProposalFilledFormValues = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Key { get; set; }

        [InverseProperty("InsuranceContractProposalFilledFormKey")]
        public List<InsuranceContractProposalFilledFormValue> InsuranceContractProposalFilledFormValues { get; set; }
    }
}
