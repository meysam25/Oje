using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Models.View
{
    public class CarExteraDiscountRangeAmountMainGrid: GlobalGrid
    {
        public int? company { get; set; }
        public string exteraDiscountTitle { get; set; }
        public string title { get; set; }
        public long? minValue { get; set; }
        public long? maxValue { get; set; }
        public bool? isActive { get; set; }
    }
}
