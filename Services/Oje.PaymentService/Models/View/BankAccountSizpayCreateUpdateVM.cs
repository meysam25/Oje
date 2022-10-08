using Oje.Infrastructure.Models;

namespace Oje.PaymentService.Models.View
{
    public class BankAccountSizpayCreateUpdateVM: GlobalSiteSetting
    {
        public int? bcId { get; set; }
        public string fistKey { get; set; }
        public string secKey { get; set; }
        public string sKey { get; set; }
        public long? terminalId { get; set; }
        public long? merchId { get; set; }
    }
}
