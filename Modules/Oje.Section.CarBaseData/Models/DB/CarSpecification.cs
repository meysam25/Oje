using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Models.DB
{
    [Table("CarSpecifications")]
    public class CarSpecification
    {
        public CarSpecification()
        {
            CarSpecificationVehicleSpecs = new();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        public decimal? CarRoomRate { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("CarSpecification")]
        public List<CarSpecificationVehicleSpec> CarSpecificationVehicleSpecs { get; set; }
    }
}
