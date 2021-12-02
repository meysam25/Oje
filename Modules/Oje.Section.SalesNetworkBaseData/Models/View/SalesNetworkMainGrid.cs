using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.SalesNetworkBaseData.Models.View
{
    public class SalesNetworkMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public string createUser { get; set; }
        public string createDate { get; set; }
        public string ppfIds { get; set; }
        public int? cIds { get; set; }
        public SalesNetworkType? type { get; set; }
        public PersonType? calceType { get; set; }
        public bool? isActive { get; set; }
    }
}
