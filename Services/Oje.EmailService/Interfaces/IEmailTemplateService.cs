using Oje.EmailService.Models.DB;
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
    public interface IEmailTemplateService
    {
        ApiResult Create(EmailTemplateCreateUpdateVM input, int? siteSettingId);
        ApiResult Delete(int? id, int? siteSettingId);
        object GetById(int? id, int? siteSettingId);
        ApiResult Update(EmailTemplateCreateUpdateVM input, int? siteSettingId);
        GridResultVM<EmailTemplateMainGridResultVM> GetList(EmailTemplateMainGrid searchInput, int? siteSettingId);
        List<EmailTemplate> GetBy(UserNotificationType type, int? siteSettingId);
    }
}
