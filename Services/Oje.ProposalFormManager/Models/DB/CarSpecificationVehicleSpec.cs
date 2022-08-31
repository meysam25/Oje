using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("CarSpecificationVehicleSpecs")]
    public class CarSpecificationVehicleSpec
    {
        public int CarSpecificationId { get; set; }
        [ForeignKey("CarSpecificationId"), InverseProperty("CarSpecificationVehicleSpecs")]
        public CarSpecification CarSpecification { get; set; }
        public int VehicleSpecId { get; set; }
        [ForeignKey("VehicleSpecId"), InverseProperty("CarSpecificationVehicleSpecs")]
        public VehicleSpec VehicleSpec { get; set; }
    }
}
