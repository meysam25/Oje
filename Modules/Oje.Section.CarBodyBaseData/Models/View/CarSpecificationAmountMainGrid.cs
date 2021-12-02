using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBodyBaseData.Models.View
{
    public class CarSpecificationAmountMainGrid: GlobalGrid
    {
        public int? company { get; set; }
        public string carSpecId { get; set; }
        public long? minValue { get; set; }
        public long? maxValue { get; set; }
        public bool? isActive { get; set; }
    }
}
