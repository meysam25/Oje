using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;

namespace Oje.ValidatedSignature.Models.View
{
    public class SmsValidationHistoryMainGrid: GlobalGrid
    {
        public string ip { get; set; }
        public string createDate { get; set; }
        public SmsValidationHistoryType? type { get; set; }
        public long? mobile { get; set; }
        public bool? isUsed { get; set; }
        public string website { get; set; }
    }
}
