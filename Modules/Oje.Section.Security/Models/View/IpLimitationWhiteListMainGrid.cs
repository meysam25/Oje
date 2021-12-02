using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.Security.Models.View
{
    public class IpLimitationWhiteListMainGrid: GlobalGrid
    {
        public string ip { get; set; }
        public bool? isActive { get; set; }
    }
}
