using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("VehicleTypes")]
    public class VehicleType
    {
        public VehicleType()
        {
            VehicleTypeCarTypes = new();
            VehicleSystemVehicleTypes = new();
            CarExteraDiscounts = new();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public int Order { get; set; }
        public int VehicleSpecCategoryId { get; set; }
        [ForeignKey("VehicleSpecCategoryId"), InverseProperty("VehicleTypes")]
        public VehicleSpecCategory VehicleSpecCategory { get; set; }

        [InverseProperty("VehicleType")]
        public List<VehicleTypeCarType> VehicleTypeCarTypes { get; set; }
        [InverseProperty("VehicleType")]
        public List<VehicleSystemVehicleType> VehicleSystemVehicleTypes { get; set; }
        [InverseProperty("VehicleType")]
        public List<CarExteraDiscount> CarExteraDiscounts { get; set; }

    }
}
