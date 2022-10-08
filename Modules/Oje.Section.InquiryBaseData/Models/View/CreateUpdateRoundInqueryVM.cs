using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;

namespace Oje.Section.InquiryBaseData.Models.View
{
    public class CreateUpdateRoundInqueryVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public string format { get; set; }
        public int? formId { get; set; }
        public RoundInqueryType? type { get; set; }
    }
}
