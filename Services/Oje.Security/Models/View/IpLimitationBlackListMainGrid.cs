using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Security.Models.View
{
    public class IpLimitationBlackListMainGrid : GlobalGrid
    {
        public string ip { get; set; }
        public bool? isActive { get; set; }
    }
}
