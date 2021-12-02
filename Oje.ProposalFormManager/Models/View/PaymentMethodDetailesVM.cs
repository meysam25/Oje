using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormManager.Models.View
{
    public class PaymentMethodDetailesVM
    {
        public PaymentMethodDetailesVM()
        {
            checkArr = new();
            paymentRequreFiles = new();
        }

        public PaymentMethodDetailesVM(List<PaymentMethodDetailesCheckVM> checkArr, List<PaymentMethodDetailesFilesVM> paymentRequreFiles)
        {
            this.checkArr = checkArr;
            this.paymentRequreFiles = paymentRequreFiles;
        }

        public string prePayment { get; set; }
        public long prePaymentLong { get; set; }
        public int debitCount { get; set; }
        public List<PaymentMethodDetailesCheckVM> checkArr { get; set; }
        public List<PaymentMethodDetailesFilesVM> paymentRequreFiles { get; set; }
    }
}
