using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;

namespace Oje.AccountService.Models.View
{
    public class CreateUpdateUserNotificationTrigerVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public UserNotificationType? type { get; set; }
        public int? roleId { get; set; }
        public long? userId { get; set; }
    }
}
