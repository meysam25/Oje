using Oje.EmailService.Models.DB;
using Oje.EmailService.Models.View;

namespace Oje.EmailService.Interfaces
{
    public interface IEmailSendingQueueService
    {
        object GetList(EmailSendingQueueMainGrid searchInput, int? siteSettingId);
        void SaveChange();
        void Create(EmailSendingQueue smsSendingQueue, int? siteSettingId);
        Task SendEmail();
    }
}
