using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceManager.Models.DB
{
    [Table("FireInsuranceCoverageCompanies")]
    public class FireInsuranceCoverageCompany
    {
        [Key]
        public long Id { get; set; }
        public int FireInsuranceCoverageId { get; set; }
        [ForeignKey("FireInsuranceCoverageId")]
        [InverseProperty("FireInsuranceCoverageCompanies")]
        public FireInsuranceCoverage FireInsuranceCoverage { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        [InverseProperty("FireInsuranceCoverageCompanies")]
        public Company Company { get; set; }

    }
}
