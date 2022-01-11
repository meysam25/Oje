using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.Sms.Models.DB;
using Oje.Sms.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Sms.Interfaces
{
    public interface ISmsSendingQueueService
    {
        object GetList(SmsSendingQueueMainGrid searchInput, int? siteSettingId);
        void SaveChange();
        void Create(SmsSendingQueue smsSendingQueue, int? siteSettingId, List<SmsLimit> smsLimits, bool? isWebsite);
        Task SendSms();
        object LoginWithSMS(RegLogSMSVM input, IpSections ipSections, int? siteSettingId);
        object ActiveCodeForResetPassword(RegLogSMSVM input, IpSections ipSections, int? siteSettingId);
    }
}
