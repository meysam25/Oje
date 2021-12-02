using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Models.DB
{
    [Table("VehicleTypes")]
    public class VehicleType
    {
        [Key]
        public int Id { get; set; }
        public int CarSpecificationId { get; set; }
        [ForeignKey("CarSpecificationId")]
        [InverseProperty("VehicleTypes")]
        public CarSpecification CarSpecification { get; set; }
        public int VehicleSystemId { get; set; }
        [ForeignKey("VehicleSystemId")]
        [InverseProperty("VehicleTypes")]
        public VehicleSystem VehicleSystem { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        public bool IsActive { get; set; }

    }
}
