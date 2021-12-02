using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Models.View
{
    public class VehicleTypeMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public string specTitle { get; set; }
        public string brandTitle { get; set; }
        public bool? isActive { get; set; }
    }
}
