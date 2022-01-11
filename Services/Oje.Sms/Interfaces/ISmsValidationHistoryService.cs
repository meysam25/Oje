using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Sms.Interfaces
{
    public interface ISmsValidationHistoryService
    {
        int GetLastSecoundFor(SmsValidationHistoryType type, IpSections ipSections, int? siteSettingId);
        int Create(IpSections ipSections, string mobile, int? siteSettingId, SmsValidationHistoryType type);
        bool ValidateBy(long mobileNumber, int smsCode, IpSections ipSections);
        string ValidatePreUsedBy(long mobileNumber, int smsCode, IpSections ipSections);
        bool IsValidPreUsed(long mobileNumber, string codeId, IpSections ipSections);
    }
}
