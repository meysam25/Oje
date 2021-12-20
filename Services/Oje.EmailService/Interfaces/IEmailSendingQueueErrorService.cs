using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.EmailService.Interfaces
{
    public interface IEmailSendingQueueErrorService
    {
        void Create(long emailSendingQueueId, DateTime createDate, string message, int? emailConfigId);
    }
}
