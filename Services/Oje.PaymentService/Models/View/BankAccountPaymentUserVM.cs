using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.PaymentService.Models.View
{
    public class BankAccountPaymentUserVM
    {
        public string bankIcon { get; set; }
        public string bankTitle { get; set; }
        public string userFullname { get; set; }
        public string accountTitle { get; set; }
        public int bankAccountId { get; set; }
    }
}
