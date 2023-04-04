using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.Sms.Models.View;

namespace Oje.Sms.Interfaces
{
    public interface ISmsValidationHistoryService
    {
        int GetLastSecoundFor(SmsValidationHistoryType type, IpSections ipSections, int? siteSettingId);
        int Create(IpSections ipSections, string mobile, int? siteSettingId, SmsValidationHistoryType type);
        bool ValidateBy(long mobileNumber, int smsCode, IpSections ipSections, int? delay, SmsValidationHistoryType sMSForCreateContract);
        string ValidatePreUsedBy(long mobileNumber, int smsCode, IpSections ipSections, int? delay, SmsValidationHistoryType loginWithSmsForContract);
        bool IsValidPreUsed(long mobileNumber, string codeId, IpSections ipSections);
        GridResultVM<SmsValidationHistoryMainGridResultVM> GetList(SmsValidationHistoryMainGrid searchInput, int? siteSettingId);
    }
}
