using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountService.Models.View
{
    public class DashboardSectionGridResultVM
    {
        public int id { get; set; }
        public int row { get; set; }
        public string type { get; set; }
        public string action { get; set; }
    }
}
