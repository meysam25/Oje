using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InsuranceContractBaseData.Models.DB
{
    [Table("InsuranceContractProposalFilledFormJsons")]
    public class InsuranceContractProposalFilledFormJson
    {
        [Key]
        public long InsuranceContractProposalFilledFormId { get; set; }
        [ForeignKey("InsuranceContractProposalFilledFormId"), InverseProperty("InsuranceContractProposalFilledFormJsons")]
        public InsuranceContractProposalFilledForm InsuranceContractProposalFilledForm { get; set; }
        [Required]
        public string JsonConfig { get; set; }
    }
}
