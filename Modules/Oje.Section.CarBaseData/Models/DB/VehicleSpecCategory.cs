using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Models.DB
{
    [Table("VehicleSpecCategories")]
    public class VehicleSpecCategory
    {
        public VehicleSpecCategory()
        {
            VehicleTypes = new();
            VehicleSpecs = new();
        }

        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Title { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("VehicleSpecCategory")]
        public List<VehicleType> VehicleTypes { get; set; }
        [InverseProperty("VehicleSpecCategory")]
        public List<VehicleSpec> VehicleSpecs { get; set; }
    }
}
