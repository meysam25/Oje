using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Models.View
{
    public class CarExteraDiscountMainGrid: GlobalGrid
    {
        public string formTitle { get; set; }
        public string title { get; set; }
        public bool? isActive { get; set; }
        public bool? isOption { get; set; }
    }
}
