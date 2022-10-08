using Oje.Infrastructure.Models;

namespace Oje.PaymentService.Models.View
{
    public class BankAccountSadadCreateUpdateVM: GlobalSiteSetting
    {
        public int? id { get; set; }
        public int? baId { get; set; }
        public string merchantId { get; set; }
        public string terminalId { get; set; }
        public string terminalKey { get; set; }
    }
}
