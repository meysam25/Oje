using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Sanab.Models.DB
{
    [Table("SanabSystemFieldVehicleSystems")]
    public class SanabSystemFieldVehicleSystem
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        public int VehicleSystemId { get; set; }
        [ForeignKey("VehicleSystemId"), InverseProperty("SanabSystemFieldVehicleSystems")]
        public VehicleSystem VehicleSystem { get; set; }
        [MaxLength(30)]
        public string Code { get; set; }
    }
}
