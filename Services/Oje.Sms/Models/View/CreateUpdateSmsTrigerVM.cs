using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;

namespace Oje.Sms.Models.View
{
    public class CreateUpdateSmsTrigerVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public UserNotificationType? type { get; set; }
        public int? roleId { get; set; }
        public long? userId { get; set; }
    }
}
