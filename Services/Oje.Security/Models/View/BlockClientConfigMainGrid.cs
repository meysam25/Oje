using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Security.Models.View
{
    public class BlockClientConfigMainGrid: GlobalGrid
    {
        public BlockClientConfigType? type { get; set; }
        public bool? isActive { get; set; }
    }
}
