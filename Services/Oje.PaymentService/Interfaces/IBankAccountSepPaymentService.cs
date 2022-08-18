using Oje.PaymentService.Models.View.Sep;

namespace Oje.PaymentService.Interfaces
{
    public interface IBankAccountSepPaymentService
    {
        Task<string> GenerateTokenAsync(string factorId, int? siteSettingId, long? loginUserId);
        Task<string> ConfirmPayment(SepCallBack input, int? siteSettingId);
    }
}
