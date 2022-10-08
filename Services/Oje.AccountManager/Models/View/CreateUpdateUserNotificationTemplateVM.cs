using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;

namespace Oje.AccountService.Models.View
{
    public class CreateUpdateUserNotificationTemplateVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public UserNotificationType? type { get; set; }
        public string subject { get; set; }
        public ProposalFilledFormUserType? pffUserType { get; set; }
        public string description { get; set; }
    }
}
