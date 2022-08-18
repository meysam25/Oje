using Oje.Infrastructure.Models;

namespace Oje.PaymentService.Models.View
{
    public class BankAccountSepMainGrid: GlobalGrid
    {
        public string bankAcount { get; set; }
        public string terminalId { get; set; }
    }
}
