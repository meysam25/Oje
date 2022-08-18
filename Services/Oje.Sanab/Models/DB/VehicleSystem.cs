using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Sanab.Models.DB
{
    [Table("VehicleSystems")]
    public class VehicleSystem
    {
        public VehicleSystem()
        {
            SanabSystemFieldVehicleSystems = new();
            VehicleSpecs = new();
            SanabTypeFieldVehicleSpecs = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Title { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("VehicleSystem")]
        public List<SanabSystemFieldVehicleSystem> SanabSystemFieldVehicleSystems { get; set; }
        [InverseProperty("VehicleSystem")]
        public List<VehicleSpec> VehicleSpecs { get; set; }
        [InverseProperty("VehicleSystem")]
        public List<SanabTypeFieldVehicleSpec> SanabTypeFieldVehicleSpecs { get; set; }
    }
}
