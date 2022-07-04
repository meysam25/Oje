using Oje.EmailService.Models.View;

namespace Oje.EmailService.Interfaces
{
    public interface IEmailSenderService
    {
        Task<EmailResult> Send(string emailAddress, string subject, string message, int? siteSettingId);
    }
}
