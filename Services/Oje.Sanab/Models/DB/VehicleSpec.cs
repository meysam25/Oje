using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Sanab.Models.DB
{
    [Table("VehicleSpecs")]
    public class VehicleSpec
    {
        public VehicleSpec()
        {
            SanabTypeFieldVehicleSpecs = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Title { get; set; }
        public int VehicleSystemId { get; set; }
        [ForeignKey("VehicleSystemId"), InverseProperty("VehicleSpecs")]
        public VehicleSystem VehicleSystem { get; set; }
        public int Order { get; set; }

        [InverseProperty("VehicleSpec")]
        public List<SanabTypeFieldVehicleSpec> SanabTypeFieldVehicleSpecs { get; set; }
    }
}
