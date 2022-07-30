using Oje.Infrastructure.Enums;
using System.Collections.Generic;

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
        public string color { get; set; }
        public List<UserNotificationType> types { get; set; }
    }
}
