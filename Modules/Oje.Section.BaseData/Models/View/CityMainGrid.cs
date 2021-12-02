using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.BaseData.Models.View
{
    public class CityMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public int? province { get; set; }
        public bool? isActive { get; set; }
    }
}
