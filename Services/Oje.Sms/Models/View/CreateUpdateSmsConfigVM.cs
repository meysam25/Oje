using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;

namespace Oje.Sms.Models.View
{
    public class CreateUpdateSmsConfigVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public string smsUsername { get; set; }
        public string smsPassword { get; set; }
        public string domain { get; set; }
        public SmsConfigType? type { get; set; }
        public string ph { get; set; }
        public bool? isActive { get; set; }
    }
}
