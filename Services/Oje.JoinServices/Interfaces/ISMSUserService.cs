using Oje.Infrastructure.Models;
using Oje.Sms.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.JoinServices.Interfaces
{
    public interface ISMSUserService
    {
        object LoginRegister(RegLogSMSVM input, IpSections ipSections, int? siteSettingId);
        object CheckIfSmsCodeIsValid(RegLogSMSVM input, IpSections ipSections, int? siteSettingId);
        object ChagePasswordAndLogin(ChangePasswordAndLoginVM input, IpSections ipSections, int? siteSettingId);
    }
}
