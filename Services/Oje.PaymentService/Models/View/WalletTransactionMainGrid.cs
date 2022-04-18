using Oje.Infrastructure.Models;
using System;

namespace Oje.PaymentService.Models.View
{
    public class WalletTransactionMainGrid: GlobalGrid
    {
        public string createDate { get; set; }
        public long? price { get; set; }
        public string descrption { get; set; }
    }
}
