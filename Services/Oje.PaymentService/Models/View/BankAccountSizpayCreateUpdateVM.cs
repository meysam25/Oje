using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.PaymentService.Models.View
{
    public class BankAccountSizpayCreateUpdateVM
    {
        public int? bcId { get; set; }
        public string fistKey { get; set; }
        public string secKey { get; set; }
        public string sKey { get; set; }
        public long? terminalId { get; set; }
        public long? merchId { get; set; }
    }
}
