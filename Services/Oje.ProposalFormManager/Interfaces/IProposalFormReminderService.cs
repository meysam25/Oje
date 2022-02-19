using Oje.Infrastructure.Models;
using Oje.ProposalFormService.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oje.ProposalFormService.Interfaces
{
    public interface IProposalFormReminderService
    {
        ApiResult Create(ReminderCreateVM input, int? siteSettingId, IpSections ipSections, long? loginUserId);
        ApiResult Delete(string id, int? siteSettingId);
        object GetById(string id, int? siteSettingId);
        ApiResult Update(ReminderCreateVM input, int? siteSettingId, long? userId);
        GridResultVM<ProposalFormReminderMainGridResultVM> GetList(ProposalFormReminderMainGrid searchInput, int? siteSettingId);
    }
}
