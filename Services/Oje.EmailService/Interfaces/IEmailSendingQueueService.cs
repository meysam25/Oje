using Oje.EmailService.Models.DB;
using Oje.EmailService.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
