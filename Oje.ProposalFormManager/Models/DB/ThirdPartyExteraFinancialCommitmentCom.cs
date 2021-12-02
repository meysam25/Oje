using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Models.DB
{
    [Table("ThirdPartyExteraFinancialCommitmentComs")]
    public class ThirdPartyExteraFinancialCommitmentCom
    {
        [Key]
        public long Id { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        [InverseProperty("ThirdPartyExteraFinancialCommitmentComs")]
        public Company Company { get; set; }
        public int ThirdPartyExteraFinancialCommitmentId { get; set; }
        [ForeignKey("ThirdPartyExteraFinancialCommitmentId")]
        [InverseProperty("ThirdPartyExteraFinancialCommitmentComs")]
        public ThirdPartyExteraFinancialCommitment ThirdPartyExteraFinancialCommitment { get; set; }

    }
}
