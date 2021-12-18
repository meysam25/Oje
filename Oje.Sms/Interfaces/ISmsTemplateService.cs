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
    public interface ISmsTemplateService
    {
        ApiResult Create(CreateUpdateSmsTemplateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Update(CreateUpdateSmsTemplateVM input, int? siteSettingId);
        GridResultVM<SmsTemplateMainGridResultVM> GetList(SmsTemplateMainGrid searchInput, int? siteSettingId);
        SmsTemplate GetBy(UserNotificationType type, int? siteSettingId);
    }
}
