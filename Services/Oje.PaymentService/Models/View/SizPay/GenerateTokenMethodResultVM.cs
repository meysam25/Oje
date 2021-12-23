using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.PaymentService.Models.View.SizPay
{
    public class GenerateTokenMethodResultVM
    {
        public string token { get; set; }
        public long merchantID { get; set; }
        public long terminalID { get; set; }
    }
}
