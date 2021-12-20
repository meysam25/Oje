using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Models.DB
{
    [Table("VehicleUsageCarTypes")]
    public class VehicleUsageCarType
    {
        public VehicleUsageCarType()
        {
            Id = Guid.NewGuid();
        }
        [Key]
        public Guid Id { get; set; }
        public int VehicleUsageId { get; set; }
        [ForeignKey("VehicleUsageId")]
        [InverseProperty("VehicleUsageCarTypes")]
        public VehicleUsage VehicleUsage { get; set; }
        public int CarTypeId { get; set; }
        [ForeignKey("CarTypeId")]
        [InverseProperty("VehicleUsageCarTypes")]
        public CarType CarType { get; set; }

    }
}
