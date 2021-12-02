using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceManager.Models.DB
{
    [Table("FireInsuranceBuildingBodies")]
    public class FireInsuranceBuildingBody
    {
        public FireInsuranceBuildingBody()
        {
            FireInsuranceRates = new List<FireInsuranceRate>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("FireInsuranceBuildingBody")]
        public List<FireInsuranceRate> FireInsuranceRates { get; set; }
    }
}
