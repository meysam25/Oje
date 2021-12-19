using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Sms.Models.View
{
    public class SmsTemplateMainGrid: GlobalGrid
    {
        public string subject { get; set; }
        public UserNotificationType? type { get; set; }
        public ProposalFilledFormUserType ? pffUserType { get; set; }
    }
}
