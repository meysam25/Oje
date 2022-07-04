using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;

namespace Oje.Sms.Models.View
{
    public class SmsValidationHistoryMainGrid: GlobalGrid
    {
        public string ip { get; set; }
        public string createDate { get; set; }
        public SmsValidationHistoryType? type { get; set; }
        public long? mobile { get; set; }
        public int? invalidCount { get; set; }
        public bool? isUsed { get; set; }
    }
}
