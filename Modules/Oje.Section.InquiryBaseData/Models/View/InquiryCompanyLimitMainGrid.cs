using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Models.View
{
    public class InquiryCompanyLimitMainGrid: GlobalGrid
    {
        public InquiryCompanyLimitType? type { get; set; }
        public int? comId { get; set; }
        public string createUser { get; set; }
        public string createDate { get; set; }
    }
}
