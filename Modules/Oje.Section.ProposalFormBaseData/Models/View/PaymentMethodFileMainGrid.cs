using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Section.ProposalFormBaseData.Models.View
{
    public class PaymentMethodFileMainGrid : GlobalGrid
    {
        public string title { get; set; }
        public int? payId { get; set; }
        public bool? isRequred { get; set; }
    }
}
