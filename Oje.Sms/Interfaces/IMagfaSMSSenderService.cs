using Oje.Sms.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Sms.Interfaces
{
    public interface IMagfaSMSSenderService
    {
        Task<SmsResult> Send(Models.DB.SmsConfig config, string destinalNumber, string message);
    }
}
