using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Sms.Interfaces
{
    public interface ISmsSendingQueueErrorService
    {
        void Create(long smsSendingQueueId, DateTime createDate, string message, int? smsConfigId);
    }
}
