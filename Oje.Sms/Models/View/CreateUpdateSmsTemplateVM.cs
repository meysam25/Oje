using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Sms.Models.View
{
    public class CreateUpdateSmsTemplateVM
    {
        public int? id { get; set; }
        public UserNotificationType? type { get; set; }
        public string subject { get; set; }
        public string description { get; set; }
        public ProposalFilledFormUserType? pffUserType { get; set; }
    }
}
