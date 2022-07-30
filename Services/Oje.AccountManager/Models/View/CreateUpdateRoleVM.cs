using Oje.Infrastructure.Enums;
using System.Collections.Generic;

namespace Oje.AccountService.Models.View
{
    public class CreateUpdateRoleVM
    {
        public long? value { get; set; }
        public int? id { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public bool? disabledOnlyMyStuff { get; set; }
        public RoleType? type { get; set; }
        public int? sitesettingId { get; set; }
        public bool? refreshGrid { get; set; }
        public List<int> formIds { get; set; }
    }
}
