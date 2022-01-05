using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Models.View
{
    public class VehicleSpecCreateUpdateVM
    {
        public int? id { get; set; }
        public string title { get; set; }
        public int? specCat { get; set; }
        public int? order { get; set; }
        public bool? isActive { get; set; }
        public int? vSystemId { get; set; }
    }
}
