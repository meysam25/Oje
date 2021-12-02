using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.InquiryBaseData.Models.View
{
    public class RoundInqueryMainGrid: GlobalGrid
    {
        public string format { get; set; }
        public string proposalFormTitle { get; set; }
        public RoundInqueryType? type { get; set; }
    }
}
