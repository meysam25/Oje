using Oje.Sms.Models.View;

namespace Oje.Sms.Interfaces
{
    public interface IIdePardazanSmsSenderService
    {
        Task<SmsResult> Send(Models.DB.SmsConfig config, string destinalNumber, string message);
    }
}
