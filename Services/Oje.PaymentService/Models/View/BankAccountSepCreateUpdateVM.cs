using Oje.Infrastructure.Models;

namespace Oje.PaymentService.Models.View
{
    public class BankAccountSepCreateUpdateVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public int? baId { get; set; }
        public string terminalId { get; set; }
    }
}
