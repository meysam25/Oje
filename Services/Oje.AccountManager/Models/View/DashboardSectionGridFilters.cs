using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Models.View
{
    public class DashboardSectionGridFilters: GlobalGrid
    {
        public DashboardSectionType? type { get; set; }
        public string action { get; set; }
        public int? pKey { get; set; }
    }
}
