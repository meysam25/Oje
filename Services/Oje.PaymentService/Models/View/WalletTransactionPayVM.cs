using Oje.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.PaymentService.Models.View
{
    public class WalletTransactionPayVM
    {
        public string id { get; set; }
        public BankAccountFactorType? type { get; set; }
    }
}
