using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Sanab.Models.DB
{
    [Table("SanabVehicleTypes")]
    public class SanabVehicleType
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Title { get; set; }
        public int VehicleTypeId { get; set; }
        [ForeignKey("VehicleTypeId"), InverseProperty("SanabVehicleTypes")]
        public VehicleType VehicleType { get; set; }
        public int Code { get; set; }
    }
}
