using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Models.View
{
    public class CreateUpdateInquiryCompanyLimitVM
    {
        public int? id { get; set; }
        public InquiryCompanyLimitType? type { get; set; }
        public List<int> comIds { get; set; }
    }
}
