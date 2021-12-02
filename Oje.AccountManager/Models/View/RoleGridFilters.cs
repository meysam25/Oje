using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountManager.Models.View
{
    public class RoleGridFilters: GlobalGrid
    {
        public string name { get; set; }
        public string title { get; set; }
        public long? value { get; set; }
        public int? siteSetting { get; set; }

    }
}
