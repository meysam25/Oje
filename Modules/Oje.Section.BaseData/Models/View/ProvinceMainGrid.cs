using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.BaseData.Models.View
{
    public class ProvinceMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public int? cityCount { get; set; }
        public bool? isActive { get; set; }
    }
}
