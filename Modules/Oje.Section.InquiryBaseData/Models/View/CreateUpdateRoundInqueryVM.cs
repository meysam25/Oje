using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Models.View
{
    public class CreateUpdateRoundInqueryVM
    {
        public int? id { get; set; }
        public string format { get; set; }
        public int? formId { get; set; }
        public RoundInqueryType? type { get; set; }
    }
}
