using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.AccountManager.Models.View
{
    public class CreateUpdateUserRoleVM
    {
        public int? id { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public bool? disabledOnlyMyStuff { get; set; }
        public RoleType? type { get; set; }
        public List<int> formIds { get; set; }

    }
}
