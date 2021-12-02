using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceManager.Models.DB
{
    [Table("FireInsuranceCoverageActivityDangerLevels")]
    public class FireInsuranceCoverageActivityDangerLevel
    {
        [Key]
        public int Id { get; set; }
        public int FireInsuranceCoverageTitleId { get; set; }
        [ForeignKey("FireInsuranceCoverageTitleId")]
        [InverseProperty("FireInsuranceCoverageActivityDangerLevels")]
        public FireInsuranceCoverageTitle FireInsuranceCoverageTitle { get; set; }
        public int DangerStep { get; set; }
        public decimal Rate { get; set; }
        public bool IsActive { get; set; }
    }
}
