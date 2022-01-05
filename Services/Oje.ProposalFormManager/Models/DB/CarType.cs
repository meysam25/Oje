using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("CarTypes")]
    public class CarType
    {
        public CarType()
        {
            VehicleUsageCarTypes = new();
            VehicleTypeCarTypes = new();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public int Order { get; set; }

        [InverseProperty("CarType")]
        public List<VehicleUsageCarType> VehicleUsageCarTypes { get; set; }
        [InverseProperty("CarType")]
        public List<VehicleTypeCarType> VehicleTypeCarTypes { get; set; }
    }
}
