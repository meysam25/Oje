using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("CarSpecifications")]
    public class CarSpecification
    {
        public CarSpecification()
        {
            ThirdPartyPassengerRates = new ();
            ThirdPartyExteraFinancialCommitments = new();
            CarSpecificationAmounts = new();
            CarSpecificationVehicleSpecs = new();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        public decimal? CarRoomRate { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("CarSpecification")]
        public List<ThirdPartyPassengerRate> ThirdPartyPassengerRates { get; set; }
        [InverseProperty("CarSpecification")]
        public List<ThirdPartyExteraFinancialCommitment> ThirdPartyExteraFinancialCommitments { get; set; }
        [InverseProperty("CarSpecification")]
        public List<CarSpecificationAmount> CarSpecificationAmounts { get; set; }
        [InverseProperty("CarSpecification")]
        public List<CarSpecificationVehicleSpec> CarSpecificationVehicleSpecs { get; set; }
    }
}
