using Oje.EmailService.Models.View;
using Oje.Infrastructure.Enums;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.EmailService.Interfaces
{
    public interface IEmailTrigerService
    {
        ApiResult Create(EmailTrigerCreateUpdateVM input, int? siteSettingID);
        ApiResult Delete(int? id, int? siteSettingID);
        object GetById(int? id, int? siteSettingID);
        ApiResult Update(EmailTrigerCreateUpdateVM input, int? siteSettingID);
        GridResultVM<EmailTrigerMainGridResultVM> GetList(EmailTrigerMainGrid searchInput, int? siteSettingID);
        void CreateEmailQue(long? userId, UserNotificationType type, List<PPFUserTypes> exteraUserList, long? objectId, string title, int? siteSettingId, object exteraParameter);
    }
}
