
namespace Oje.PaymentService.Interfaces
{
    public interface IProposalFilledFormService
    {
        void UpdateTraceCode(long objectId, string traceNo);
        long ValidateForPayment(int? siteSettingId, long id);
    }
}
