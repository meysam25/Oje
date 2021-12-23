using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.PaymentService.Models.View
{
    public class BankAccountCreateUpdateVM
    {
        public int? id { get; set; }
        public int? bankId { get; set; }
        public string title { get; set; }
        public long? cardNo { get; set; }
        public string shabaNo { get; set; }
        public long hesabNo { get; set; }
        public long? userId { get; set; }
        public bool? isForPayment { get; set; }
        public bool? isActive { get; set; }
    }
}
