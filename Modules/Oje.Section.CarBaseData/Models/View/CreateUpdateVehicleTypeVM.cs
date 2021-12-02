using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Models.View
{
    public class CreateUpdateVehicleTypeVM
    {
        public int? id { get; set; }
        public int? carSpecId { get; set; }
        public int? carVehicleSystemId { get; set; }
        public string title { get; set; }
        public bool? isActive { get; set; }
        public string carSpecId_Title { get; set; }
        public string carVehicleSystemId_Title { get; set; }
    }
}
