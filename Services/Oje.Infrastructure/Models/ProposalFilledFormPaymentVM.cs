using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Infrastructure.Models
{
    public class ProposalFilledFormPaymentVM
    {
        public long price { get; set; }
        public string fullName { get; set; }
        public DateTime payDate { get; set; }
        public string traceCode { get; set; }
    }
}
