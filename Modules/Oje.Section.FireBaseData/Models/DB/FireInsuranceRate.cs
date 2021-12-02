using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.FireBaseData.Models.DB
{
    [Table("FireInsuranceRates")]
    public class FireInsuranceRate
    {
        public FireInsuranceRate()
        {
            FireInsuranceRateCompanies = new List<FireInsuranceRateCompany>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public int FireInsuranceBuildingBodyId { get; set; }
        [ForeignKey("FireInsuranceBuildingBodyId")]
        [InverseProperty("FireInsuranceRates")]
        public FireInsuranceBuildingBody FireInsuranceBuildingBody { get; set; }
        public int FireInsuranceBuildingTypeId { get; set; }
        [ForeignKey("FireInsuranceBuildingTypeId")]
        [InverseProperty("FireInsuranceRates")]
        public FireInsuranceBuildingType FireInsuranceBuildingType { get; set; }
        public long FromPrice { get; set; }
        public long ToPrice { get; set; }
        public decimal Rate { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("FireInsuranceRate")]
        public List<FireInsuranceRateCompany> FireInsuranceRateCompanies { get; set; }
    }
}
