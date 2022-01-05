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
        public string title { get; set; }
        public int? specCatId { get; set; }
        public int? order { get; set; }
        public bool? isActive { get; set; }
        public List<int> carTypeIds { get; set; }
    }
}
