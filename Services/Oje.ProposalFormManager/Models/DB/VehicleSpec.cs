using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("VehicleSpecs")]
    public class VehicleSpec
    {
        public VehicleSpec()
        {
            CarSpecificationVehicleSpecs = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Title { get; set; }
        public int VehicleSpecCategoryId { get; set; }
        [ForeignKey("VehicleSpecCategoryId"), InverseProperty("VehicleSpecs")]
        public VehicleSpecCategory VehicleSpecCategory { get; set; }
        public int? VehicleTypeId { get; set; }
        [ForeignKey("VehicleTypeId"), InverseProperty("VehicleSpecs")]
        public VehicleType VehicleType { get; set; }
        public bool IsActive { get; set; }
        public int Order { get; set; }
        public int VehicleSystemId { get; set; }
        [ForeignKey("VehicleSystemId"), InverseProperty("VehicleSpecs")]
        public VehicleSystem VehicleSystem { get; set; }

        [InverseProperty("VehicleSpec")]
        public List<CarSpecificationVehicleSpec> CarSpecificationVehicleSpecs { get; set; }

    }
}
