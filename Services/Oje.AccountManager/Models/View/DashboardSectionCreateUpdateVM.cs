using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Models.View
{
    public class DashboardSectionCreateUpdateVM
    {
        public int? id { get; set; }
        public int? pKey { get; set; }
        public string @class { get; set; }
        public DashboardSectionType? type { get; set; }
        public long? actionId { get; set; }
        public int? catId { get; set; }
        public int? order { get; set; }
    }
}
