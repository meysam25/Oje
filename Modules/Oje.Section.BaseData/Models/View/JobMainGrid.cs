using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.BaseData.Models.View
{
    public class JobMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public int? dlId { get; set; }
        public bool? isActive { get; set; }
    }
}
