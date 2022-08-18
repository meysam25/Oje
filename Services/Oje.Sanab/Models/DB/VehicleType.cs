using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.Sanab.Models.DB
{
    [Table("VehicleTypes")]
    public class VehicleType
    {
        public VehicleType()
        {
            SanabVehicleTypes = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Title { get; set; }
        public int Order { get; set; }

        [InverseProperty("VehicleType")]
        public List<SanabVehicleType> SanabVehicleTypes { get; set; }
    }
}
