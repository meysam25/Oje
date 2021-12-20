using Oje.EmailService.Models.DB;
using Oje.EmailService.Models.View;
using Oje.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.EmailService.Interfaces
{
    public interface IEmailConfigService
    {
        ApiResult Create(EmailConfigCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Update(EmailConfigCreateUpdateVM input, int? siteSettingId);
        GridResultVM<EmailConfigMainGridResultVM> GetList(EmailConfigMainGrid searchInput, int? siteSettingId);
        EmailConfig GetActive(int? siteSettingId);
    }
}
