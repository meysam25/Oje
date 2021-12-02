using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.FireBaseData.Models.DB
{
    [Table("FireInsuranceRateCompanies")]
    public class FireInsuranceRateCompany
    {
        [Key]
        public long Id { get; set; }
        public int FireInsuranceRateId { get; set; }
        [ForeignKey("FireInsuranceRateId")]
        [InverseProperty("FireInsuranceRateCompanies")]
        public FireInsuranceRate FireInsuranceRate { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        [InverseProperty("FireInsuranceRateCompanies")]
        public Company Company { get; set; }
    }
}
