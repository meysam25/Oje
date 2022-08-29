using Oje.Infrastructure.Enums;

namespace Oje.PaymentService.Models.View
{
    public class BankAccountFactorVM
    {
        public BankAccountFactorType Type { get; set; }
        public long ObjectId { get; set; }
        public long Price { get; set; }
        public string TargetLink { get; set; }
        public long? UserId { get; set; }
        public string errorMessage { get; set; }
    }
}
