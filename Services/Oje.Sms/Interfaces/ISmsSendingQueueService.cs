using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.Sms.Models.DB;
using Oje.Sms.Models.View;

namespace Oje.Sms.Interfaces
{
    public interface ISmsSendingQueueService
    {
        object GetList(SmsSendingQueueMainGrid searchInput, int? siteSettingId);
        void SaveChange();
        void Create(SmsSendingQueue smsSendingQueue, int? siteSettingId, List<SmsLimit> smsLimits, bool? isWebsite, bool? ignoreIp = null);
        Task SendSms();
        object LoginWithSMS(RegLogSMSVM input, IpSections ipSections, int? siteSettingId, SmsValidationHistoryType? smsValidationHistoryType);
        object ActiveCodeForResetPassword(RegLogSMSVM input, IpSections ipSections, int? siteSettingId);
        string CreateFilledSendedCode(RegLogSMSVM regLogSMSVM, IpSections ipSections, int? siteSettingId, SmsValidationHistoryType loginWithSmsForContract);
    }
}
