using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Section.CarBaseData.Models.DB
{
    [Table("VehicleTypes")]
    public class VehicleType
    {
        public VehicleType()
        {
            VehicleTypeCarTypes = new();
            VehicleSystemVehicleTypes = new();
            CarExteraDiscounts = new();
            VehicleSpecs = new();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public int Order { get; set; }
        public int VehicleSpecCategoryId { get; set; }
        [ForeignKey("VehicleSpecCategoryId"), InverseProperty("VehicleTypes")]
        public VehicleSpecCategory VehicleSpecCategory { get; set; }

        [InverseProperty("VehicleType")]
        public List<VehicleTypeCarType> VehicleTypeCarTypes { get; set; }
        [InverseProperty("VehicleType")]
        public List<VehicleSystemVehicleType> VehicleSystemVehicleTypes { get; set; }
        [InverseProperty("VehicleType")]
        public List<CarExteraDiscount> CarExteraDiscounts { get; set; }
        [InverseProperty("VehicleType")]
        public List<VehicleSpec> VehicleSpecs { get; set; }

    }
}
