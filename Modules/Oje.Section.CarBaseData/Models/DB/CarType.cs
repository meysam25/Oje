using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Models.DB
{
    [Table("CarTypes")]
    public class CarType
    {
        public CarType()
        {
            VehicleSystems = new List<VehicleSystem>();
            VehicleUsageCarTypes = new List<VehicleUsageCarType>();
            CarExteraDiscounts = new List<CarExteraDiscount>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("CarType")]
        public List<VehicleSystem> VehicleSystems { get; set; }
        [InverseProperty("CarType")]
        public List<VehicleUsageCarType> VehicleUsageCarTypes { get; set; }
        [InverseProperty("CarType")]
        public List<CarExteraDiscount> CarExteraDiscounts { get; set; }
    }
}
