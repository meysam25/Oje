using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarThirdBaseData.Models.DB
{
    [Table("CarSpecifications")]
    public class CarSpecification
    {
        public CarSpecification()
        {
            ThirdPartyRates = new List<ThirdPartyRate>();
            ThirdPartyExteraFinancialCommitments = new List<ThirdPartyExteraFinancialCommitment>();
            ThirdPartyPassengerRates = new List<ThirdPartyPassengerRate>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [InverseProperty("CarSpecification")]
        public List<ThirdPartyRate> ThirdPartyRates { get; set; }
        [InverseProperty("CarSpecification")]
        public List<ThirdPartyExteraFinancialCommitment> ThirdPartyExteraFinancialCommitments { get; set; }
        [InverseProperty("CarSpecification")]
        public List<ThirdPartyPassengerRate> ThirdPartyPassengerRates { get; set; }
    }
}
