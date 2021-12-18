using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("CarSpecifications")]
    public class CarSpecification
    {
        public CarSpecification()
        {
            VehicleTypes = new ();
            ThirdPartyPassengerRates = new ();
            ThirdPartyExteraFinancialCommitments = new();
            CarSpecificationAmounts = new();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        public decimal? CarRoomRate { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("CarSpecification")]
        public List<VehicleType> VehicleTypes { get; set; }
        [InverseProperty("CarSpecification")]
        public List<ThirdPartyPassengerRate> ThirdPartyPassengerRates { get; set; }
        [InverseProperty("CarSpecification")]
        public List<ThirdPartyExteraFinancialCommitment> ThirdPartyExteraFinancialCommitments { get; set; }
        [InverseProperty("CarSpecification")]
        public List<CarSpecificationAmount> CarSpecificationAmounts { get; set; }
    }
}
