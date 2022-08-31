using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("ThirdPartyRequiredFinancialCommitmentCompanies")]
    public class ThirdPartyRequiredFinancialCommitmentCompany
    {
        [Key]
        public long Id { get; set; }
        public int ThirdPartyRequiredFinancialCommitmentId { get; set; }
        [ForeignKey("ThirdPartyRequiredFinancialCommitmentId")]
        [InverseProperty("ThirdPartyRequiredFinancialCommitmentCompanies")]
        public ThirdPartyRequiredFinancialCommitment ThirdPartyRequiredFinancialCommitment { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        [InverseProperty("ThirdPartyRequiredFinancialCommitmentCompanies")]
        public Company Company { get; set; }

    }
}
