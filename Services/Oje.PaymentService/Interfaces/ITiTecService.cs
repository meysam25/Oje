using Oje.PaymentService.Models.View;
using Oje.PaymentService.Models.View.TiTec;

namespace Oje.PaymentService.Interfaces
{
    public interface ITiTecService
    {
        Task<AccessTokenResult> LoginAsync(int? siteSettingId);
        Task<FactorResultVM> CreateFactorAsync(FactorVM input, int? siteSettingId);
        Task<FactorLinkResultVM> CreateFactorLinkAsync(FactorLinkVM input, int? siteSettingId);
        Task<FactorPayInquiryResultVM> InquiryFactorAsync(FactorPayInquiryVM input, int? siteSettingId);
        Task<string> PayAsync(string bankAccountFactorId, long? loginUserId, int? siteSettingId);
        Task InquiryFactorForWorkerAsync();
        Task<string> ConfirmPayment(TiTecConfirmPaymentInput input, int? siteSettingId);
    }
}
