using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceManager.Models.DB
{
    [Table("FireInsuranceCoverages")]
    public class FireInsuranceCoverage
    {
        public FireInsuranceCoverage()
        {
            FireInsuranceCoverageCompanies = new List<FireInsuranceCoverageCompany>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public decimal Rate { get; set; }
        public int FireInsuranceCoverageTitleId { get; set; }
        [ForeignKey("FireInsuranceCoverageTitleId")]
        [InverseProperty("FireInsuranceCoverages")]
        public FireInsuranceCoverageTitle FireInsuranceCoverageTitle { get; set; }
        public int ProposalFormId { get; set; }
        [ForeignKey("ProposalFormId")]
        [InverseProperty("FireInsuranceCoverages")]
        public ProposalForm ProposalForm { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("FireInsuranceCoverage")]
        public List<FireInsuranceCoverageCompany> FireInsuranceCoverageCompanies { get; set; }
    }
}
