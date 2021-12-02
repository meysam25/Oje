using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.FireInsuranceManager.Models.DB
{
    [Table("FireInsuranceCoverageTitles")]
    public class FireInsuranceCoverageTitle
    {
        public FireInsuranceCoverageTitle()
        {
            FireInsuranceCoverages = new List<FireInsuranceCoverage>();
            FireInsuranceCoverageActivityDangerLevels = new List<FireInsuranceCoverageActivityDangerLevel>();
            FireInsuranceCoverageCityDangerLevels = new List<FireInsuranceCoverageCityDangerLevel>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public FireInsuranceCoverageEffectOn EffectOn { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("FireInsuranceCoverageTitle")]
        public List<FireInsuranceCoverage> FireInsuranceCoverages { get; set; }
        [InverseProperty("FireInsuranceCoverageTitle")]
        public List<FireInsuranceCoverageActivityDangerLevel> FireInsuranceCoverageActivityDangerLevels { get; set; }
        [InverseProperty("FireInsuranceCoverageTitle")]
        public List<FireInsuranceCoverageCityDangerLevel> FireInsuranceCoverageCityDangerLevels { get; set; }
    }
}
