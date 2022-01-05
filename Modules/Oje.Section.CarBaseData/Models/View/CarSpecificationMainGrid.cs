using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.CarBaseData.Models.View
{
    public class CarSpecificationMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public bool? isActive { get; set; }
        public decimal? carRomeRate { get; set; }
        public string specs { get; set; }
    }
}
