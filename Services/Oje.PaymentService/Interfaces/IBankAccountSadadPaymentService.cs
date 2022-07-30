using Oje.PaymentService.Models.View.Sadad;

namespace Oje.PaymentService.Interfaces
{
    public interface IBankAccountSadadPaymentService
    {
        Task<string> PaymentRequestAsync(string bankAccountFactorId, int? siteSettingId, long? loginUserId);
        Task<string> ConfirmPayment(PurchaseResult input, int? siteSettingId);
    }
}
