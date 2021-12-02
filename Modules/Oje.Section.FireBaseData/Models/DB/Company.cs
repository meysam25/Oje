using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.FireBaseData.Models.DB
{
    [Table("Companies")]
    public class Company
    {
        public Company()
        {
            FireInsuranceCoverageCompanies = new List<FireInsuranceCoverageCompany>();
            FireInsuranceRateCompanies = new List<FireInsuranceRateCompany>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [InverseProperty("Company")]
        public List<FireInsuranceCoverageCompany> FireInsuranceCoverageCompanies { get; set; }
        [InverseProperty("Company")]
        public List<FireInsuranceRateCompany> FireInsuranceRateCompanies { get; set; }
    }
}
