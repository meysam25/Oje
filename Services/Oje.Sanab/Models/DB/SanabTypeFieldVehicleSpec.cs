using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Sanab.Models.DB
{
    [Table("SanabTypeFieldVehicleSpecs")]
    public class SanabTypeFieldVehicleSpec
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        public int VehicleSpecId { get; set; }
        [ForeignKey("VehicleSpecId"), InverseProperty("SanabTypeFieldVehicleSpecs")]
        public VehicleSpec VehicleSpec { get; set; }
        public int VehicleSystemId { get; set; }
        [ForeignKey("VehicleSystemId"), InverseProperty("SanabTypeFieldVehicleSpecs")]
        public VehicleSystem VehicleSystem { get; set; }
        public string Code { get; set; }
    }
}
