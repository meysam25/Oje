using Oje.Infrastructure.Models;

namespace Oje.PaymentService.Models.View
{
    public class BankAccountSadadMainGrid: GlobalGrid
    {
        public string bankAcount { get; set; }
        public string merchantId { get; set; }
        public string terminalId { get; set; }
    }
}
