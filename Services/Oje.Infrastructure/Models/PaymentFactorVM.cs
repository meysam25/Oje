using Oje.Infrastructure.Enums;

namespace Oje.Infrastructure.Models
{
    public class PaymentFactorVM
    {
        public BankAccountFactorType type { get; set; }
        public long objectId { get; set; }
        public long price { get; set; }
        public string returnUrl { get; set; }
        public long? userId { get; set; }
        public string errorMessage { get; set; }
    }
}
