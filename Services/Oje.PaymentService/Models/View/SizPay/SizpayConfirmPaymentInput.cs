using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.PaymentService.Models.View.SizPay
{
    public class SizpayConfirmPaymentInput: SizPayGlobalRespone
    {
        public string MerchantID { get; set; }
        public string TerminalID { get; set; }
        public string InvoiceNo { get; set; }
        public string OrderID { get; set; }
        public string Token { get; set; }
        public string AppExtraInf { get; set; }
        public string ExtraInf { get; set; }

    }
}
