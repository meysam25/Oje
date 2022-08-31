using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("ThirdPartyPassengerRates")]
    public class ThirdPartyPassengerRate
    {
        public ThirdPartyPassengerRate()
        {
            ThirdPartyPassengerRateCompanies = new List<ThirdPartyPassengerRateCompany>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public int CarSpecificationId { get; set; }
        [ForeignKey("CarSpecificationId")]
        [InverseProperty("ThirdPartyPassengerRates")]
        public CarSpecification CarSpecification { get; set; }
        public int Year { get; set; }
        public decimal Rate { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("ThirdPartyPassengerRate")]
        public List<ThirdPartyPassengerRateCompany> ThirdPartyPassengerRateCompanies { get; set; }
    }
}
