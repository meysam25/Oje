using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.PaymentService.Models.View.SizPay
{
    public class SizPayConfirmPaymentPost
    {
        public string MerchantID { get; set; }
        public string TerminalID { get; set; }
        public string SignData { get; set; }
        public string Token { get; set; }
    }
}
