using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.PaymentService.Models.View
{
    public class BankAccountSizpayMainGrid: GlobalGrid
    {
        public string title { get; set; }
        public long? terminalId { get; set; }
        public long? merchandId { get; set; }
        public string shbaNo { get; set; }
    }
}
