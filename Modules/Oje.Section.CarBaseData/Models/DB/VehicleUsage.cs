using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Models.DB
{
    [Table("VehicleUsages")]
    public class VehicleUsage
    {
        public VehicleUsage()
        {
            VehicleUsageCarTypes = new List<VehicleUsageCarType>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        public decimal? BodyPercent { get; set; }
        public decimal? ThirdPartyPercent { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("VehicleUsage")]
        public List<VehicleUsageCarType> VehicleUsageCarTypes { get; set; }
    }
}
