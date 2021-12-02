using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.FireBaseData.Models.DB
{
    [Table("FireInsuranceCoverageCityDangerLevels")]
    public class FireInsuranceCoverageCityDangerLevel
    {
        [Key]
        public int Id { get; set; }
        public int FireInsuranceCoverageTitleId { get; set; }
        [ForeignKey("FireInsuranceCoverageTitleId")]
        [InverseProperty("FireInsuranceCoverageCityDangerLevels")]
        public FireInsuranceCoverageTitle FireInsuranceCoverageTitle { get; set; }
        public int DangerStep { get; set; }
        public decimal Rate { get; set; }
        public bool IsActive { get; set; }
    }
}
