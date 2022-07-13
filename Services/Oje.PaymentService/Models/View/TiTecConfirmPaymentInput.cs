
namespace Oje.PaymentService.Models.View
{
    public class TiTecConfirmPaymentInput
    {
        public long? Amount { get; set; }
        public string FactorNo { get; set; }
        public string FactorId { get; set; }
        public string TrackingNo { get; set; }
    }
}
