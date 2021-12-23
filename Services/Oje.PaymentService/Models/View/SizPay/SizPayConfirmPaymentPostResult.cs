using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.PaymentService.Models.View.SizPay
{
    public class SizPayConfirmPaymentPostResult : SizPayGlobalRespone
    {
        public string MerchantID { get; set; }
        public string TerminalID { get; set; }
        public string OrderID { get; set; }
        public string InvoiceNo { get; set; }
        public string ExtraInf { get; set; }
        public string Token { get; set; }
        public long Amount { get; set; }
        public string CardNo { get; set; }
        public string TransDate { get; set; }
        public string TransNo { get; set; }
        public long RefNo { get; set; }
        public string TraceNo { get; set; }
    }
}
