using Oje.Infrastructure.Models;
using Oje.Sms.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Sms.Interfaces
{
    public interface ISmsConfigService
    {
        ApiResult Create(CreateUpdateSmsConfigVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Update(CreateUpdateSmsConfigVM input, int? siteSettingId);
        GridResultVM<SmsConfigMainGridResultVM> GetList(SmsConfigMainGrid searchInput, int? siteSettingId);
        Models.DB.SmsConfig GetActive(int? siteSettingId);
    }
}
