using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.ProposalFormBaseData.Models.View
{
    public class PaymentMethodMainGrid: GlobalGrid
    {
        public string form { get; set; }
        public string title { get; set; }
        public int? comId { get; set; }
        public PaymentMethodType? type { get; set; }
        public bool? isActive { get; set; }
        public bool? isDefault { get; set; }
    }
}
