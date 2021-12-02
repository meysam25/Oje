using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Models.DB
{
    [Table("VehicleSystems")]
    public class VehicleSystem
    {
        public VehicleSystem()
        {
            VehicleTypes = new List<VehicleType>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        public int CarTypeId { get; set; }
        [ForeignKey("CarTypeId")]
        [InverseProperty("VehicleSystems")]
        public CarType CarType { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("VehicleSystem")]
        public List<VehicleType> VehicleTypes { get; set; }
    }
}
