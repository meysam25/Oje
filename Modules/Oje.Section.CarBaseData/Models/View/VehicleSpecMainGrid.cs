using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Models.View
{
    public class VehicleSpecMainGrid:GlobalGrid
    {
        public string title { get; set; }
        public int? specCat { get; set; }
        public bool? isActive { get; set; }
        public string vSystem { get; set; }
    }
}
