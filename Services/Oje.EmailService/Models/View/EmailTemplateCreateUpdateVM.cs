using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;

namespace Oje.EmailService.Models.View
{
    public class EmailTemplateCreateUpdateVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public UserNotificationType? type { get; set; }
        public string subject { get; set; }
        public string description { get; set; }
        public ProposalFilledFormUserType? pffUserType { get; set; }
    }
}
