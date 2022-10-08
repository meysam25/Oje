using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;

namespace Oje.Sms.Models.View
{
    public class CreateUpdateSmsTemplateVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public UserNotificationType? type { get; set; }
        public string subject { get; set; }
        public string description { get; set; }
        public ProposalFilledFormUserType? pffUserType { get; set; }
    }
}
