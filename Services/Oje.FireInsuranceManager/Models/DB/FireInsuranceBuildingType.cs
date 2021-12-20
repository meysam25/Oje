using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceService.Models.DB
{
    [Table("FireInsuranceBuildingTypes")]
    public class FireInsuranceBuildingType
    {
        public FireInsuranceBuildingType()
        {
            FireInsuranceRates = new List<FireInsuranceRate>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("FireInsuranceBuildingType")]
        public List<FireInsuranceRate> FireInsuranceRates { get; set; }
    }
}
