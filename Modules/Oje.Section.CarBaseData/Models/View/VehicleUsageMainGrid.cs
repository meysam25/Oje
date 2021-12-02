using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Models.View
{
    public class VehicleUsageMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public int? carTypes { get; set; }
        public decimal? bodyPercent { get; set; }
        public decimal? thirdPercent { get; set; }
        public bool? isActive { get; set; }
    }
}
