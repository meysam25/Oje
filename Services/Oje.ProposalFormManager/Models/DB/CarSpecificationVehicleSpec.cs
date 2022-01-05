using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
