using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Security.Models.View
{
    public class BlockClientConfigCreateUpdateVM
    {
        public int? id { get; set; }
        public BlockClientConfigType? type { get; set; }
        public int? maxSoftware { get; set; }
        public int? maxSuccessSoftware { get; set; }
        public int? maxFirewall { get; set; }
        public int? maxSuccessFirewall { get; set; }
        public bool? isActive { get; set; }
    }
}
