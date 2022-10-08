using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using System.Collections.Generic;

namespace Oje.Section.InquiryBaseData.Models.View
{
    public class CreateUpdateInquiryCompanyLimitVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public InquiryCompanyLimitType? type { get; set; }
        public List<int> comIds { get; set; }
    }
}
