using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using Oje.Sms.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.Sms.Interfaces
{
    public interface ISmsTrigerService
    {
        ApiResult Create(CreateUpdateSmsTrigerVM input, int? siteSettingID);
        ApiResult Delete(int? id, int? siteSettingID);
        object GetById(int? id, int? siteSettingID);
        ApiResult Update(CreateUpdateSmsTrigerVM input, int? siteSettingID);
        GridResultVM<SmsTrigerMainGridResultVM> GetList(SmsTrigerMainGrid searchInput, int? siteSettingID);
        void CreateSmsQue(long? userId, UserNotificationType type, List<PPFUserTypes> exteraUserList, long? objectId, string title, int? siteSettingId, object exteraParameter);
    }
}
