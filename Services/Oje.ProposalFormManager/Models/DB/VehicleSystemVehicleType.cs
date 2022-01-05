using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("VehicleSystemVehicleTypes")]
    public class VehicleSystemVehicleType
    {
        public int VehicleSystemId { get; set; }
        [ForeignKey("VehicleSystemId"), InverseProperty("VehicleSystemVehicleTypes")]
        public VehicleSystem VehicleSystem { get; set; }
        public int VehicleTypeId { get; set; }
        [ForeignKey("VehicleTypeId"), InverseProperty("VehicleSystemVehicleTypes")]
        public VehicleType VehicleType { get; set; }
    }
}
