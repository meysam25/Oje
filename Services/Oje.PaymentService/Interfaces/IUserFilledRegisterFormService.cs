namespace Oje.PaymentService.Interfaces
{
    public interface IUserFilledRegisterFormService
    {
        void UpdateTraceCode(long id, int? siteSettingId, string traceNo);
    }
}
