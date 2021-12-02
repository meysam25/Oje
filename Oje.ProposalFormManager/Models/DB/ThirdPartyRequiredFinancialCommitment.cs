using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Models.DB
{
    [Table("ThirdPartyRequiredFinancialCommitments")]
    public class ThirdPartyRequiredFinancialCommitment
    {
        public ThirdPartyRequiredFinancialCommitment()
        {
            ThirdPartyRequiredFinancialCommitmentCompanies = new List<ThirdPartyRequiredFinancialCommitmentCompany>();
            ThirdPartyExteraFinancialCommitments = new List<ThirdPartyExteraFinancialCommitment>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public long Price { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("ThirdPartyRequiredFinancialCommitment")]
        public List<ThirdPartyRequiredFinancialCommitmentCompany> ThirdPartyRequiredFinancialCommitmentCompanies { get; set; }
        [InverseProperty("ThirdPartyRequiredFinancialCommitment")]
        public List<ThirdPartyExteraFinancialCommitment> ThirdPartyExteraFinancialCommitments { get; set; }
    }
}
