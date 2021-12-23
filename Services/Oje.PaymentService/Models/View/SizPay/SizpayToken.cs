using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.PaymentService.Models.View.SizPay
{
    internal class SizpayToken
    {
        public string MerchantID { get; set; }
        public string TerminalID { get; set; }
        public string SignData { get; set; }
        public long Amount { get; set; }
        public string DocDate { get; set; }
        public string OrderID { get; set; }
        public string ReturnURL { get; set; }
        public string ExtraInf { get; set; }
        public string InvoiceNo { get; set; }
        public string AppExtraInf { get; set; }
    }
}
