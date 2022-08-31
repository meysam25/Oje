using System.ComponentModel.DataAnnotations.Schema;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("VehicleTypeCarTypes")]
    public class VehicleTypeCarType
    {
        public int VehicleTypeId { get; set; }
        [ForeignKey("VehicleTypeId"), InverseProperty("VehicleTypeCarTypes")]
        public VehicleType VehicleType { get; set; }
        public int CarTypeId { get; set; }
        [ForeignKey("CarTypeId"), InverseProperty("VehicleTypeCarTypes")]
        public CarType CarType { get; set; }
    }
}
